using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CircularBuffer;
using RingBuffer4chan;

namespace DeathMatchConsoleApp
{
	internal static class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length >= 1 && args[0] == "justrun")
			{
				// Pass `justrun` as arg to avoid BenchmarkDotNet benchmarks and simply debug/profile
				JustRun();
			}
			else
			{
				BenchmarkRunner.Run<Benchmarks.AddOne>();
				BenchmarkRunner.Run<Benchmarks.AddOneTakeOne>();
				BenchmarkRunner.Run<Benchmarks.AddMultiple>();
				BenchmarkRunner.Run<Benchmarks.AddMultipleTakeMultiple>();
			}
		}

		private static void JustRun()
		{
			static TimeSpan RunWithTimeMeasurement(Action runBenchmark, int times, string benchmarkDescription)
			{
				Console.WriteLine($"Running benchmark {times} times: {benchmarkDescription}");
				
				var start = DateTime.Now;
				runBenchmark();
				var took = DateTime.Now - start;

				Console.WriteLine($"Took {took}");
				return took;
			}

			{
				var benchmark = new Benchmarks.AddOne();
				benchmark.Times = 100;
				const int timesToRun = 1_000_000;
				var tookCB = RunWithTimeMeasurement(() => benchmark.WithCircularBuffer(), timesToRun,
					$"{benchmark.GetType().Name}.{nameof(benchmark.WithCircularBuffer)} with {nameof(benchmark.Times)}={benchmark.Times}");
				var tookRB = RunWithTimeMeasurement(() => benchmark.WithRingBuffer(), timesToRun,
					$"{benchmark.GetType().Name}.{nameof(benchmark.WithRingBuffer)} with {nameof(benchmark.Times)}={benchmark.Times}");

				double tookCBBenchmark = tookCB.Ticks;
				double tookRBBenchmark = tookRB.Ticks;
				if (tookCBBenchmark <= tookRBBenchmark)
				{
					double slower = (tookRBBenchmark - tookCBBenchmark) / tookCBBenchmark;
					Console.WriteLine($"RingBuffer was {slower} times slower.");
				}
				else
				{
					double faster = (tookCBBenchmark - tookRBBenchmark) / tookRBBenchmark;
					Console.WriteLine($"RingBuffer was {faster} times faster.");
				}
			}
		}
	}

	[MemoryDiagnoser]
	public static class Benchmarks
	{
		public class AddOne
		{
			private readonly CircularBuffer<int> _circularBuffer = new(128);
			private readonly RingBuffer<int> _ringBuffer = new(128);

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
			private readonly CircularBuffer<int> _circularBuffer = new(128);
			private readonly RingBuffer<int> _ringBuffer = new(128);

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
			private static readonly int[] NumbersToAdd =
				Enumerable.Range(0, 1000)
					.Select((v, i) => v + i).ToArray();

			private readonly CircularBuffer<int> _circularBuffer = new(128);
			private readonly RingBuffer<int> _ringBuffer = new(128);

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
			private static readonly int[] NumbersToAdd =
				Enumerable.Range(0, 1000)
					.Select((v, i) => v + i).ToArray();

			private readonly CircularBuffer<int> _circularBuffer = new(128);
			private readonly RingBuffer<int> _ringBuffer = new(128);

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