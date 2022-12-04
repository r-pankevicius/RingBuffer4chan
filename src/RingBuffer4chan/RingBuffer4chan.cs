//#define DUMP_STATE_ON_EXCEPTION
//#define CHECKINMULTIPLE_ONE_BY_ONE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RingBuffer4chan
{
	/// <summary>
	/// Circular buffer, circular queue, cyclic buffer or ring buffer.
	/// <para>
	/// See https://en.wikipedia.org/wiki/Circular_buffer
	/// </para>
	/// </summary>
	/// <typeparam name="T">Type of buffer elements</typeparam>
	public class RingBuffer<T> : IEnumerable<T>
	{
		/// <summary>
		/// Array to be treated as circular buffer.
		/// </summary>
		/// <remarks>
		/// Its size is double of requested buffer capacity to avoid the need
		/// allocating a new array when trying to join two spans: one at the end and
		/// another at the start.
		/// </remarks>
		private readonly T[] _buffer;

		public RingBuffer(int capacity)
		{
			if (capacity < 1)
				throw new ArgumentOutOfRangeException($"{nameof(capacity)} must be >= 1, but was {capacity}");

			Capacity = capacity;
			_buffer = new T[capacity * 2];
			ReadIndex = WriteIndex = 0;
		}

		public RingBuffer(int capacity, IEnumerable<T> items) : this(capacity)
		{
			if (items is null)
				throw new ArgumentNullException(nameof(items));

			// Performance loss because of ToArray() because I don't know about span over IEnumerable yet
			var itemsArray = items.ToArray();
			if (itemsArray.Length > Capacity)
				throw new ArgumentOutOfRangeException(nameof(items), $"{itemsArray.Length} items won't fit in the buffer of capacity {capacity}.");

			Array.Copy(sourceArray: itemsArray, destinationArray: _buffer, length: itemsArray.Length);
			WriteIndex = itemsArray.Length;
		}

		/// <summary>
		/// internal for unit tests.
		/// </summary>
		internal RingBuffer(int capacity, InternalState initialState) : this(capacity)
		{
			ReadIndex = initialState.ReadIndex;
			WriteIndex = initialState.WriteIndex;
		}

		/// <summary>
		/// internal for unit tests.
		/// </summary>
		internal RingBuffer(int capacity, IEnumerable<T> items, InternalState initialState) :
			this(capacity, items)
		{
			ReadIndex = initialState.ReadIndex;
			WriteIndex = initialState.WriteIndex;
		}

		/// <summary>Read index is where the buffer contents start.</summary>
		/// <remarks>internal getter for unit tests, should be replaced with GetInternalStateSnapshot()</remarks>
		internal int ReadIndex
		{
			get;
			private set;
		}

		/// <summary>
		/// Write index is where the buffer contents end.
		/// <para>
		/// The range between <see cref="ReadIndex"/> and <see cref="WriteIndex"/>
		/// are current buffer contents.
		/// </para>
		/// <para>
		/// When <see cref="ReadIndex"/> == <see cref="WriteIndex"/> the buffer is empty.
		/// </para>
		/// </summary>
		/// <remarks>internal getter for unit tests, should be replaced with GetInternalStateSnapshot()</remarks>
		internal int WriteIndex
		{
			get;
			// A place to catch a nasty bug by setting a breakpoint with the condition:
			// value > Capacity * 2
			private set;
		}

		/// <summary>Maximum buffer capacity.</summary>
		public int Capacity { get; private set; }

		/// <summary>Number of items currently in the buffer.</summary>
		public int Size => WriteIndex - ReadIndex;

		public void CheckIn(T item)
		{
#if DUMP_STATE_ON_EXCEPTION
			try
#endif
			{
				if (WriteIndex >= _buffer.Length)
				{
					// Move all block without first item to the buffer start
					int size = Size;
					var bufferSpan = _buffer.AsSpan();
					var sourceBuffer = bufferSpan[(ReadIndex + 1)..];
					var targetBuffer = bufferSpan[0..(size - 1)];
					sourceBuffer.CopyTo(targetBuffer);
					ReadIndex = 0;
					_buffer[size - 1] = item;
					WriteIndex = size;
					return;
				}
				else if (Size >= Capacity)
				{
					// Drop the first item because contents won't fit
					ReadIndex++;
				}

				_buffer[WriteIndex++] = item;
			}
#if DUMP_STATE_ON_EXCEPTION
			catch
			{
				DumpInternalState();
				throw;
			}
#endif
		}

		public void CheckInMultiple(ReadOnlySpan<T> items)
		{
#if !CHECKINMULTIPLE_ONE_BY_ONE
			int newItemsLength = items.Length;
			var bufferSpan = _buffer.AsSpan();

			if (WriteIndex + newItemsLength < bufferSpan.Length)
			{
				// Enough buffer place to copy all items to the end
				items.CopyTo(bufferSpan[WriteIndex..]);
				WriteIndex += newItemsLength;
				if (Size > Capacity)
				{
					// Need to push read index forward removing some older items
					ReadIndex = WriteIndex - Capacity;
				}
			}
			else
			{
				// Need to move buffer to the start
				int newSize = Size + newItemsLength;
				if (newSize > Capacity)
				{
					// Some or all of old items will be overwriten
					if (newItemsLength >= Capacity)
					{
						// All old items will be overwritten
						items[(newItemsLength - Capacity)..].CopyTo(bufferSpan);
					}
					else
					{
						// Only some of old items will be overwritten
						int oldItemsRemaining = Capacity - newItemsLength;
						var remainingSpan = bufferSpan[(ReadIndex + Size - oldItemsRemaining)..WriteIndex];
						remainingSpan.CopyTo(bufferSpan);
						items.CopyTo(bufferSpan[oldItemsRemaining..]);
					}

					ReadIndex = 0;
					WriteIndex = Capacity;
				}
				else
				{
					var remainsInBuffer = bufferSpan[ReadIndex..WriteIndex];
					remainsInBuffer.CopyTo(bufferSpan);
					items.CopyTo(bufferSpan[remainsInBuffer.Length..]);
					ReadIndex = 0;
					WriteIndex = remainsInBuffer.Length + newItemsLength;
				}
			}

#else
			// Very inefficient, but it will allow to start with unit tests
			foreach (T item in items)
				CheckIn(item);
#endif
		}

		public T SniffFirst()
		{
			if (Size == 0)
				throw new InvalidOperationException("Buffer is empty.");

			return _buffer[ReadIndex];
		}

		public T SniffLast()
		{
			if (Size == 0)
				throw new InvalidOperationException("Buffer is empty.");

			return _buffer[WriteIndex - 1];
		}

		public T SniffAt(int offsetFromStart)
		{
			if (offsetFromStart >= Size)
				throw new ArgumentOutOfRangeException($"Item at index {offsetFromStart} is beyond the buffer size {Size}.");

			return _buffer[ReadIndex + offsetFromStart];
		}

		public ReadOnlySpan<T> SniffMultiple(int numberOfItems)
		{
			if (numberOfItems <= 0)
				throw new ArgumentOutOfRangeException($"{nameof(numberOfItems)} must be > 0, was {numberOfItems}.");

			if (numberOfItems > Size)
				throw new ArgumentOutOfRangeException(
					$"Can't get more items than buffer size ({Size}). Items requested: {numberOfItems}.");

			var bufferSpan = _buffer.AsSpan();
			var requestedSpan = bufferSpan[ReadIndex..(ReadIndex + numberOfItems)];

			return requestedSpan;
		}

		public ReadOnlySpan<T> SniffAll()
		{
			var bufferSpan = _buffer.AsSpan();
			var requestedSpan = bufferSpan[ReadIndex..(ReadIndex + Size)];
			return requestedSpan;
		}

		public T CheckOut()
		{
			if (Size == 0)
				throw new InvalidOperationException("Buffer is empty.");

			T result = _buffer[ReadIndex++];
			if (ReadIndex == _buffer.Length)
			{
				ReadIndex = WriteIndex = 0;
			}

			return result;
		}

		public T[] CheckOutMultiple(int numberOfItems)
		{
			var requestedSpan = SniffMultiple(numberOfItems);
			ReadIndex += numberOfItems;
			return requestedSpan.ToArray();
		}

		public void Clear() =>
			ReadIndex = WriteIndex = 0;

		#region IEnumerable<T> implementation

		public IEnumerator<T> GetEnumerator()
		{
			var array = SniffAll().ToArray();
			return ((IEnumerable<T>)array).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			var array = SniffAll().ToArray();
			return array.GetEnumerator();
		}

		#endregion

#if DUMP_STATE_ON_EXCEPTION
		private void DumpInternalState()
		{
			string separatorLine = new string('-', 80);

			Console.WriteLine(separatorLine);

			Console.WriteLine($"{GetType().Name}.{nameof(Capacity)} = {Capacity}");
			Console.WriteLine($"{GetType().Name}.{nameof(ReadIndex)} = {ReadIndex}");
			Console.WriteLine($"{GetType().Name}.{nameof(WriteIndex)} = {WriteIndex}");

			Console.WriteLine(separatorLine);
		}
#endif

		internal readonly struct InternalState
		{
			public InternalState(int readIndex, int writeIndex)
			{
				ReadIndex = readIndex;
				WriteIndex = writeIndex;
			}
			
			public int ReadIndex { get; }
			
			public int WriteIndex { get; }
		}
	}
}
