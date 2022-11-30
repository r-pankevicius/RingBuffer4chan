using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CircularBuffer;
using RingBuffer4chan;

namespace DeathMatchConsoleApp
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			BenchmarkRunner.Run<Benchmarks.AddOne>();
			BenchmarkRunner.Run<Benchmarks.AddMultiple>();
		}
	}

	[MemoryDiagnoser]
	public static class Benchmarks
	{
		public class AddOne
		{
			readonly CircularBuffer<int> _circularBuffer = new(128);
			readonly RingBuffer<int> _ringBuffer = new(128);

			[Params(100, 1000, 10000)]
			public int Times { get; set; }

			[Benchmark(Baseline = true)]
			public void WithCircularBuffer()
			{
				_circularBuffer.PushBack(10);
			}

			[Benchmark]
			public void WithRingBuffer()
			{
				_ringBuffer.CheckIn(10);
			}
		}

		public class AddMultiple
		{
			readonly static int[] NumbersToAdd =
				Enumerable.Range(0, 1000)
					.Select((v, i) => v + i).ToArray();

			readonly CircularBuffer<int> _circularBuffer = new(128);
			readonly RingBuffer<int> _ringBuffer = new(128);

			[Params(10, 50, 100)]
			public int HowMany { get; set; }

			[Benchmark(Baseline = true)]
			public void WithCircularBuffer()
			{
				var numbersSpan = NumbersToAdd.AsSpan();

				for (int idx = 0; idx < HowMany; idx++)
				{
					_circularBuffer.PushBack(numbersSpan[idx]);
				}
			}

			[Benchmark]
			public void WithRingBuffer()
			{
				var numbersSpan = NumbersToAdd.AsSpan();
				_ringBuffer.CheckInMultiple(numbersSpan[0..HowMany]);
			}
		}
	}
}