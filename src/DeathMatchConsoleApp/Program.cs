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
			if (args.Length == 1 && args[0] == "justrun")
			{
				// Pass `justrun` as arg to avoid benchmarks
				// and debug exception got by running them
				var addOne = new Benchmarks.AddOne();
				foreach (int times in new int[] { 10, 100, 1000 })
				{
					addOne.Times = times;
					addOne.WithRingBuffer();
				}
			}
			else
			{
				BenchmarkRunner.Run<Benchmarks.AddOne>();
				BenchmarkRunner.Run<Benchmarks.AddOneTakeOne>();
				BenchmarkRunner.Run<Benchmarks.AddMultiple>();
				BenchmarkRunner.Run<Benchmarks.AddMultipleTakeMultiple>();
			}
		}
	}

	[MemoryDiagnoser]
	public static class Benchmarks
	{
		public class AddOne
		{
			readonly CircularBuffer<int> _circularBuffer = new(128);
			readonly RingBuffer<int> _ringBuffer = new(128);

			[Params(10, 100, 1000)]
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

		public class AddOneTakeOne
		{
			readonly CircularBuffer<int> _circularBuffer = new(128);
			readonly RingBuffer<int> _ringBuffer = new(128);

			[Params(10, 100, 1000)]
			public int Times { get; set; }

			[Benchmark(Baseline = true)]
			public void WithCircularBuffer()
			{
				_circularBuffer.PushBack(10);
				_ = _circularBuffer.Front();
			}

			[Benchmark]
			public void WithRingBuffer()
			{
				_ringBuffer.CheckIn(10);
				_ = _ringBuffer.CheckOut();
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
				int howMany = HowMany;

				for (int idx = 0; idx < howMany; idx++)
				{
					_circularBuffer.PushBack(numbersSpan[idx]);
				}
			}

			[Benchmark]
			public void WithRingBuffer()
			{
				var numbersSpan = NumbersToAdd.AsSpan();
				int howMany = HowMany;

				_ringBuffer.CheckInMultiple(numbersSpan[0..howMany]);
			}
		}

		public class AddMultipleTakeMultiple
		{
			readonly static int[] NumbersToAdd =
				Enumerable.Range(0, 1000)
					.Select((v, i) => v + i).ToArray();

			readonly CircularBuffer<int> _circularBuffer = new(128);
			readonly RingBuffer<int> _ringBuffer = new(128);

			[Params(10, 50, 100)]
			public int HowMany { get; set; }

			private int TwoThirds => HowMany * 2 / 3;

			[Benchmark(Baseline = true)]
			public void WithCircularBuffer()
			{
				var numbersSpan = NumbersToAdd.AsSpan();
				int howMany = HowMany, twoThirds = TwoThirds;

				for (int idx = 0; idx < howMany; idx++)
				{
					_circularBuffer.PushBack(numbersSpan[idx]);
				}

				for (int idx = 0; idx < twoThirds; idx++)
				{
					_ = _circularBuffer.Front();
				}
			}

			[Benchmark]
			public void WithRingBuffer()
			{
				var numbersSpan = NumbersToAdd.AsSpan();
				int howMany = HowMany, twoThirds = TwoThirds;

				_ringBuffer.CheckInMultiple(numbersSpan[0..howMany]);
				_ = _ringBuffer.CheckOutMultiple(twoThirds);
			}
		}
	}
}