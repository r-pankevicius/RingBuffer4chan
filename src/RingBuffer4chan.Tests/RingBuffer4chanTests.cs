using FluentAssertions;
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
	}
}
