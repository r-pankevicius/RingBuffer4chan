using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace RingBuffer4chan
{
	public class RingBufferTests
	{
		[Fact]
		public void CheckBuffer2StateAfterCtor()
		{
			RingBuffer<int> ringBuffer = new(capacity: 2);
			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(0);
			ringBuffer.ReadIndex.Should().Be(0);
			ringBuffer.WriteIndex.Should().Be(0);
		}

		[Fact]
		public void CheckBufferStateAfterCheckIn1Item()
		{
			RingBuffer<int> ringBuffer = new(capacity: 2);
			ringBuffer.CheckIn(1);
			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(1);
			ringBuffer.ReadIndex.Should().Be(0);
			ringBuffer.WriteIndex.Should().Be(1);

			ringBuffer.SniffFirst().Should().Be(1);
			ringBuffer.SniffLast().Should().Be(1);
		}

		[Fact]
		public void CheckBuffer2StateAfterCheckIn2Items()
		{
			RingBuffer<int> ringBuffer = new(capacity: 2);
			ringBuffer.CheckIn(1);
			ringBuffer.CheckIn(2);
			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(2);
			ringBuffer.ReadIndex.Should().Be(0);
			ringBuffer.WriteIndex.Should().Be(2);

			ringBuffer.SniffFirst().Should().Be(1);
			ringBuffer.SniffLast().Should().Be(2);
		}

		[Fact]
		public void CheckBuffer2StateAfterCheckIn3Items()
		{
			RingBuffer<int> ringBuffer = new(capacity: 2);
			ringBuffer.CheckIn(1);
			ringBuffer.CheckIn(2);
			ringBuffer.CheckIn(3);
			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(2);
			ringBuffer.ReadIndex.Should().Be(1);
			ringBuffer.WriteIndex.Should().Be(3);

			ringBuffer.SniffFirst().Should().Be(2);
			ringBuffer.SniffLast().Should().Be(3);
		}

		[Fact]
		public void CheckBuffer2StateAfterCheckIn4Items()
		{
			RingBuffer<int> ringBuffer = new(capacity: 2);
			ringBuffer.CheckIn(1);
			ringBuffer.CheckIn(2);
			ringBuffer.CheckIn(3);
			ringBuffer.CheckIn(4);
			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(2);
			ringBuffer.ReadIndex.Should().Be(2);
			ringBuffer.WriteIndex.Should().Be(4);

			ringBuffer.SniffFirst().Should().Be(3);
			ringBuffer.SniffLast().Should().Be(4);
		}

		[Fact]
		public void CheckBuffer2StateAfterCheckIn5Items()
		{
			RingBuffer<int> ringBuffer = new(capacity: 2);
			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(0);

			ringBuffer.CheckIn(1);
			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(1);

			ringBuffer.CheckIn(2);
			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(2);

			ringBuffer.CheckIn(3);
			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(2);

			ringBuffer.CheckIn(4);
			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(2);

			ringBuffer.CheckIn(5);
			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(2);
			ringBuffer.ReadIndex.Should().Be(0);
			ringBuffer.WriteIndex.Should().Be(2);

			ringBuffer.SniffFirst().Should().Be(4);
			ringBuffer.SniffLast().Should().Be(5);
		}

		[Fact]
		public void CheckBuffer2StateAfterCheckIn6Items()
		{
			RingBuffer<int> ringBuffer = new(capacity: 2);
			ringBuffer.CheckIn(1);
			ringBuffer.CheckIn(2);
			ringBuffer.CheckIn(3);
			ringBuffer.CheckIn(4);
			ringBuffer.CheckIn(5);
			ringBuffer.CheckIn(6);

			ringBuffer.Capacity.Should().Be(2);
			ringBuffer.Size.Should().Be(2);
			ringBuffer.ReadIndex.Should().Be(1);
			ringBuffer.WriteIndex.Should().Be(3);

			ringBuffer.SniffFirst().Should().Be(5);
			ringBuffer.SniffLast().Should().Be(6);
		}

		[Fact]
		public void AddOneTakeOneResetsBufferWhenCapacityX2Reached()
		{
			const int capacity = 3;
			RingBuffer<int> _ringBuffer = new(capacity);

			for (int i = 0; i < capacity * 10; i++)
			{
				_ringBuffer.CheckIn(i);
				int value = _ringBuffer.CheckOut();
				value.Should().Be(i);
			}
		}

		[Fact]
		public void CheckinMultipleWhenBufferSizeWillBeReached()
		{
			const int capacity = 3; // buffers size will be 6
			RingBuffer<int> _ringBuffer = new(capacity, new int[] { 1, 2 });

			_ringBuffer.Size.Should().Be(2);
			_ringBuffer.ReadIndex.Should().Be(0);
			_ringBuffer.WriteIndex.Should().Be(2);
			_ringBuffer.SniffFirst().Should().Be(1);
			_ringBuffer.SniffLast().Should().Be(2);

			_ringBuffer.CheckInMultiple(new int[] { 3, 4, 5 });
			_ringBuffer.SniffFirst().Should().Be(3);
			_ringBuffer.SniffLast().Should().Be(5);

			_ = _ringBuffer.CheckOutMultiple(2);
			_ringBuffer.SniffFirst().Should().Be(5);
			_ringBuffer.SniffLast().Should().Be(5);

			_ringBuffer.Size.Should().Be(1);
			_ringBuffer.ReadIndex.Should().Be(4);
			_ringBuffer.WriteIndex.Should().Be(5);

			_ringBuffer.CheckInMultiple(new int[] { 6, 7 });
		}

		[Fact]
		public void TrickyOne_AnInstanceOfTheNextRandom()
		{
			var initialState = new RingBuffer<int>.InternalState(
				readIndex: 150, writeIndex: 250);
			RingBuffer<int> _ringBuffer = new(128, initialState);

			// this
			//{RingBuffer4chan.RingBuffer<int>}
			//    Capacity: 128
			//    ReadIndex: 150
			//     Size: 100
			//     WriteIndex: 250
			_ringBuffer.Capacity.Should().Be(128);
			_ringBuffer.ReadIndex.Should().Be(150);
			_ringBuffer.Size.Should().Be(100);
			_ringBuffer.WriteIndex.Should().Be(250);

			int[] numbersToAdd = Enumerable.Range(0, 10).ToArray();
			_ringBuffer.CheckInMultiple(numbersToAdd);
		}

		[Fact]
		public void RandomTestWithWithCheckInMultipleCheckOutMultiple()
		{
			RingBuffer<int> _ringBuffer = new(128);

			int howMany = 10, twoThirds = howMany * 2 / 3;
			int[] numbersToAdd = Enumerable.Range(0, howMany).ToArray();

			for (int loopIdx = 0; loopIdx < 100; loopIdx++)
			{
				var numbersSpan = numbersToAdd.AsSpan();

				_ringBuffer.CheckInMultiple(numbersSpan[0..howMany]);
				_ = _ringBuffer.CheckOutMultiple(twoThirds);
			}
		}

		[Fact]
		public void SomeTest1()
		{
			RingBuffer<char> _ringBuffer = new(3, "ABC");
			_ringBuffer.CheckIn('D');
		}

		[Fact]
		public void SomeTest2()
		{
			RingBuffer<char> _ringBuffer = new(3, "ABC");
			_ = _ringBuffer.CheckOut();
			_ringBuffer.CheckIn('D');
			_ringBuffer.CheckIn('E');
		}

		[Fact]
		public void SomeTest3()
		{
			RingBuffer<char> _ringBuffer = new(3, "ABC");
			_ = _ringBuffer.CheckOutMultiple(2);
			_ringBuffer.CheckIn('D');
			_ringBuffer.CheckIn('E');
		}

		[Fact]
		public void SomeTest4()
		{
			RingBuffer<char> _ringBuffer = new(3, "ABC");
			_ = _ringBuffer.CheckOutMultiple(2);
			_ringBuffer.CheckInMultiple("DEF");
		}
	}
}
