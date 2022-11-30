# 2022-11-30 at b2262c5

## AddOne
```
|             Method | Times |     Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------- |------ |---------:|----------:|----------:|------:|--------:|
| WithCircularBuffer |   100 | 1.390 ns | 0.0197 ns | 0.0184 ns |  1.00 |    0.00 |
|     WithRingBuffer |   100 | 2.160 ns | 0.0093 ns | 0.0077 ns |  1.55 |    0.02 |
|                    |       |          |           |           |       |         |
| WithCircularBuffer |  1000 | 1.496 ns | 0.0113 ns | 0.0094 ns |  1.00 |    0.00 |
|     WithRingBuffer |  1000 | 2.172 ns | 0.0100 ns | 0.0089 ns |  1.45 |    0.01 |
|                    |       |          |           |           |       |         |
| WithCircularBuffer | 10000 | 1.415 ns | 0.0129 ns | 0.0114 ns |  1.00 |    0.00 |
|     WithRingBuffer | 10000 | 2.930 ns | 0.0461 ns | 0.0385 ns |  2.07 |    0.03 |```

## AddMultiple
```
|             Method | HowMany |      Mean |    Error |   StdDev | Ratio | RatioSD |
|------------------- |-------- |----------:|---------:|---------:|------:|--------:|
| WithCircularBuffer |      10 |  19.68 ns | 0.244 ns | 0.524 ns |  1.00 |    0.00 |
|     WithRingBuffer |      10 |  32.87 ns | 0.414 ns | 0.345 ns |  1.68 |    0.07 |
|                    |         |           |          |          |       |         |
| WithCircularBuffer |      50 |  94.17 ns | 0.994 ns | 0.881 ns |  1.00 |    0.00 |
|     WithRingBuffer |      50 | 159.93 ns | 2.400 ns | 2.128 ns |  1.70 |    0.03 |
|                    |         |           |          |          |       |         |
| WithCircularBuffer |     100 | 183.33 ns | 2.799 ns | 3.437 ns |  1.00 |    0.00 |
|     WithRingBuffer |     100 | 317.03 ns | 2.282 ns | 1.906 ns |  1.72 |    0.04 |
```
