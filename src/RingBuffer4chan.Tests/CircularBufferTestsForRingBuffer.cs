using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace RingBuffer4chan
{
	/// <summary>
	/// Try to do all tests that are in CircularBuffer-CSharp tests suite.
	/// </summary>
	/// <remarks>
	/// https://github.com/joaoportela/CircularBuffer-CSharp/blob/master/CircularBuffer.Tests/CircularBufferTests.cs
	/// </remarks>
	public class CircularBufferTestsForRingBuffer
	{
		[Fact]
		// CircularBuffer_GetEnumeratorConstructorCapacity_ReturnsEmptyCollection
		public void Constructor_WithNoItemsAdded_ReturnsEmptyCollection()
		{
			var buffer = new RingBuffer<string>(5);
			buffer.SniffAll().ToArray().Should().BeEmpty();
		}

		[Fact]
		// CircularBuffer_ConstructorSizeIndexAccess_CorrectContent
		public void ConstructorSizeIndexAccess_CorrectContent()
		{
			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3 });

			buffer.Capacity.Should().Be(5);
			buffer.Size.Should().Be(4);
			for (int i = 0; i < 4; i++)
			{
				buffer.SniffAt(i).Should().Be(i);
			}
		}

		[Fact]
		// Constructor_ExceptionWhenSourceIsLargerThanCapacity
		public void Constructor_ExceptionWhenSourceIsLargerThanCapacity()
		{
			Action act = () => { _ = new RingBuffer<int>(3, new[] { 0, 1, 2, 3 }); };
			act.Should().Throw<ArgumentOutOfRangeException>();
		}

		[Fact]
		// GetEnumeratorConstructorDefinedArray_CorrectContent
		public void GetEnumeratorConstructorDefinedArray_CorrectContent()
		{
			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3 });

			int idx = 0;
			foreach (var item in buffer)
			{
				item.Should().Be(idx);
				idx++;
			}
		}

		[Fact]
		// PushBack_CorrectContent
		public void CheckIn_CorrectContent()
		{
			var buffer = new RingBuffer<int>(5);

			for (int i = 0; i < 5; i++)
			{
				buffer.CheckIn(i);
			}

			buffer.SniffFirst().Should().Be(0);
			buffer.SniffLast().Should().Be(4);

			for (int i = 0; i < 5; i++)
			{
				buffer.SniffAt(i).Should().Be(i);
			}
		}

		[Fact]
		// PushBackOverflowingBuffer_CorrectContent
		public void CheckInOverflowingBuffer_CorrectContent()
		{
			var buffer = new RingBuffer<int>(5);

			for (int i = 0; i < 10; i++)
			{
				buffer.CheckIn(i);
			}

			buffer.SniffAll().ToArray().Should().
				BeEquivalentTo(new[] { 5, 6, 7, 8, 9 });
		}

		[Fact]
		// GetEnumeratorOverflowedArray_CorrectContent
		// ToArrayOverflowedBuffer_CorrectContent
		public void GetEnumeratorOverflowedArray_CorrectContent()
		{
			var buffer = new RingBuffer<int>(5);

			for (int i = 0; i < 10; i++)
			{
				buffer.CheckIn(i);
			}

			int x = 5;
			foreach (var item in buffer)
			{
				item.Should().Be(x);
				x++;
			}
		}

		[Fact]
		// ToArrayConstructorDefinedArray_CorrectContent
		public void ConstructorDefinedArray_CorrectContent()
		{
			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3 });

			buffer.SniffAll().ToArray().Should().
				BeEquivalentTo(new[] { 0, 1, 2, 3 });
		}

		// Tests ain't needed:
		// ToArraySegmentsConstructorDefinedArray_CorrectContent
		// ToArraySegmentsOverflowedBuffer_CorrectContent

#if false // PushFront (CheckInFirst) is not supported. I don't feel I'll need it at all.
		[Fact]
		// PushFront_CorrectContent
		public void PushFront_CorrectContent()
		{
			var buffer = new RingBuffer<int>(5);

			for (int i = 0; i < 5; i++)
			{
				buffer.PushFront(i);
			}

			Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 4, 3, 2, 1, 0 }));
		}

		[Fact]
		// PushFrontAndOverflow_CorrectContent
		public void PushFrontAndOverflow_CorrectContent()
		{
			var buffer = new RingBuffer<int>(5);

			for (int i = 0; i < 10; i++)
			{
				buffer.PushFront(i);
			}

			Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 9, 8, 7, 6, 5 }));
		}
#endif

		[Fact]
		// Front_CorrectItem
		public void SniffFirst_CorrectItem()
		{
			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
			buffer.SniffFirst().Should().Be(0);
		}

		[Fact]
		// Back_CorrectItem
		public void SniffLast_CorrectItem()
		{
			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
			buffer.SniffLast().Should().Be(4);
		}

		[Fact]
		// BackOfBufferOverflowByOne_CorrectItem
		public void BackOfBufferOverflowByOne_CorrectItem()
		{
			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
			buffer.CheckIn(42);

			buffer.SniffAll().ToArray().Should().
				BeEquivalentTo(new[] { 1, 2, 3, 4, 42 });
			buffer.SniffLast().Should().Be(42);
		}

		[Fact]
		// Front_EmptyBufferThrowsException
		public void SniffFirst_EmptyBufferThrowsException()
		{
			var buffer = new RingBuffer<int>(5);

			Action act = () => { _ = buffer.SniffFirst(); };
			act.Should().Throw<InvalidOperationException>().WithMessage("*buffer*empty*"); ;
		}

		[Fact]
		// Back_EmptyBufferThrowsException
		public void SniffLast_EmptyBufferThrowsException()
		{
			var buffer = new RingBuffer<int>(5);

			Action act = () => { _ = buffer.SniffLast(); };
			act.Should().Throw<InvalidOperationException>().WithMessage("*buffer*empty*"); ;
		}

#if false // PopBack (CheckOutLast) is not supported. I don't feel I'll need it at all.
		[Fact]
		// PopBack_RemovesBackElement
		public void PopBack_RemovesBackElement()
		{
			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

			Assert.That(buffer.Size, Is.EqualTo(5));

			buffer.PopBack();

			Assert.That(buffer.Size, Is.EqualTo(4));
			Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 1, 2, 3 }));
		}

		[Fact]
		// PopBackInOverflowBuffer_RemovesBackElement
		public void PopBackInOverflowBuffer_RemovesBackElement()
		{
			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
			buffer.PushBack(5);

			Assert.That(buffer.Size, Is.EqualTo(5));
			Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));

			buffer.PopBack();

			Assert.That(buffer.Size, Is.EqualTo(4));
			Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 1, 2, 3, 4 }));
		}
#endif

		[Fact]
		// PopFront_RemovesBackElement
		public void CheckOut_RemovesFirstElement()
		{
			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

			buffer.Size.Should().Be(5);

			int firstItem = buffer.CheckOut();
			firstItem.Should().Be(0);

			buffer.Size.Should().Be(4);
			buffer.SniffAll().ToArray().Should().
				BeEquivalentTo(new[] { 1, 2, 3, 4 });
		}

#if false // PushFront, PopFront are not supported
		[Fact]
		// PopFrontInOverflowBuffer_RemovesBackElement
		public void PopFrontInOverflowBuffer_RemovesBackElement()
		{
			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });
			buffer.PushFront(5);

			Assert.That(buffer.Size, Is.EqualTo(5));
			Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 5, 0, 1, 2, 3 }));

			buffer.PopFront();

			Assert.That(buffer.Size, Is.EqualTo(4));
			Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 1, 2, 3 }));
		}
#endif

#if false // Index setter (write access) is not supported
		[Fact]
		// SetIndex_ReplacesElement
		public void SetIndex_ReplacesElement()
		{
			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

			buffer[1] = 10;
			buffer[3] = 30;

			Assert.That(buffer.ToArray(), Is.EqualTo(new[] { 0, 10, 2, 30, 4 }));
		}
#endif

#if false // PopFront is not supported
		[Fact]
		// WithDifferentSizeAndCapacity_BackReturnsLastArrayPosition
		public void WithDifferentSizeAndCapacity_BackReturnsLastArrayPosition()
		{
			// test to confirm this issue does not happen anymore:
			// https://github.com/joaoportela/CircularBuffer-CSharp/issues/2

			var buffer = new RingBuffer<int>(5, new[] { 0, 1, 2, 3, 4 });

			buffer.PopFront(); // (make size and capacity different)

			Assert.That(buffer.Back(), Is.EqualTo(4));
		}
#endif

		[Fact]
		// Clear_ClearsContent
		public void Clear_ClearsContent()
		{
			var buffer = new RingBuffer<int>(5, new[] { 4, 3, 2, 1, 0 });

			buffer.Clear();

			buffer.Size.Should().Be(0);
			buffer.Capacity.Should().Be(5);
			buffer.SniffAll().ToArray().Should().
				BeEquivalentTo(Array.Empty<int>());
		}

		[Fact]
		// Clear_WorksNormallyAfterClear
		public void Clear_WorksNormallyAfterClear()
		{
			var buffer = new RingBuffer<int>(5, new[] { 4, 3, 2, 1, 0 });

			buffer.Clear();
			for (int i = 0; i < 5; i++)
			{
				buffer.CheckIn(i);
			}

			buffer.SniffFirst().Should().Be(0);
			for (int i = 0; i < 5; i++)
			{
				buffer.SniffAt(i).Should().Be(i);
			}
		}
	}
}
