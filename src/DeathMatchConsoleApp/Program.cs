using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CircularBuffer;
using RingBuffer4chan;
using System;

namespace DeathMatchConsoleApp
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			BenchmarkRunner.Run<Benchmarks.JustAdd>();
		}
	}

	[MemoryDiagnoser]
	public static class Benchmarks
	{
		public class JustAdd
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
	}
}