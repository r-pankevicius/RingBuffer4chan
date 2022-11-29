# 2022-11-29 at b2262c5

```
JustAdd

|             Method | Times |     Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------- |------ |---------:|----------:|----------:|------:|--------:|
| WithCircularBuffer |   100 | 1.421 ns | 0.0213 ns | 0.0188 ns |  1.00 |    0.00 |
|     WithRingBuffer |   100 | 2.334 ns | 0.0721 ns | 0.0675 ns |  1.64 |    0.05 |
|                    |       |          |           |           |       |         |
| WithCircularBuffer |  1000 | 1.486 ns | 0.0126 ns | 0.0106 ns |  1.00 |    0.00 |
|     WithRingBuffer |  1000 | 2.218 ns | 0.0215 ns | 0.0202 ns |  1.49 |    0.02 |
|                    |       |          |           |           |       |         |
| WithCircularBuffer | 10000 | 1.481 ns | 0.0147 ns | 0.0130 ns |  1.00 |    0.00 |
|     WithRingBuffer | 10000 | 2.392 ns | 0.0320 ns | 0.0299 ns |  1.62 |    0.02 |
```