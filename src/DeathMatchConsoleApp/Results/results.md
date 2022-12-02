# 2022-11-30 at b2262c5

## AddOne
```
|             Method | Times |     Mean |     Error |    StdDev |   Median | Ratio | RatioSD |
|------------------- |------ |---------:|----------:|----------:|---------:|------:|--------:|
| WithCircularBuffer |    10 | 1.551 ns | 0.0577 ns | 0.1179 ns | 1.507 ns |  1.00 |    0.00 |
|     WithRingBuffer |    10 | 2.073 ns | 0.0653 ns | 0.1433 ns | 2.027 ns |  1.35 |    0.14 |
|                    |       |          |           |           |          |       |         |
| WithCircularBuffer |   100 | 1.641 ns | 0.0227 ns | 0.0189 ns | 1.633 ns |  1.00 |    0.00 |
|     WithRingBuffer |   100 | 2.043 ns | 0.0057 ns | 0.0053 ns | 2.043 ns |  1.25 |    0.01 |
|                    |       |          |           |           |          |       |         |
| WithCircularBuffer |  1000 | 1.396 ns | 0.0385 ns | 0.0321 ns | 1.381 ns |  1.00 |    0.00 |
|     WithRingBuffer |  1000 | 1.927 ns | 0.0085 ns | 0.0076 ns | 1.926 ns |  1.38 |    0.03 |
```

## AddOneTakeOne
```
|             Method | Times |     Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------- |------ |---------:|----------:|----------:|------:|--------:|
| WithCircularBuffer |    10 | 2.242 ns | 0.0133 ns | 0.0124 ns |  1.00 |    0.00 |
|     WithRingBuffer |    10 | 3.883 ns | 0.0112 ns | 0.0105 ns |  1.73 |    0.01 |
|                    |       |          |           |           |       |         |
| WithCircularBuffer |   100 | 2.607 ns | 0.0431 ns | 0.0403 ns |  1.00 |    0.00 |
|     WithRingBuffer |   100 | 4.431 ns | 0.0070 ns | 0.0062 ns |  1.70 |    0.03 |
|                    |       |          |           |           |       |         |
| WithCircularBuffer |  1000 | 2.723 ns | 0.0238 ns | 0.0199 ns |  1.00 |    0.00 |
|     WithRingBuffer |  1000 | 4.680 ns | 0.1177 ns | 0.2751 ns |  1.77 |    0.14 |

// * Warnings *
MultimodalDistribution
  AddOneTakeOne.WithRingBuffer: Default -> It seems that the distribution is bimodal (mValue = 3.56)
```

## AddMultiple
```
|             Method | HowMany |      Mean |    Error |   StdDev | Ratio |
|------------------- |-------- |----------:|---------:|---------:|------:|
| WithCircularBuffer |      10 |  19.84 ns | 0.059 ns | 0.052 ns |  1.00 |
|     WithRingBuffer |      10 |  27.87 ns | 0.034 ns | 0.030 ns |  1.40 |
|                    |         |           |          |          |       |
| WithCircularBuffer |      50 |  98.60 ns | 0.508 ns | 0.450 ns |  1.00 |
|     WithRingBuffer |      50 | 142.33 ns | 1.435 ns | 1.272 ns |  1.44 |
|                    |         |           |          |          |       |
| WithCircularBuffer |     100 | 173.00 ns | 1.025 ns | 0.909 ns |  1.00 |
|     WithRingBuffer |     100 | 278.32 ns | 0.810 ns | 0.677 ns |  1.61 |
```

## AddMultipleTakeMultiple
```
|             Method | HowMany |      Mean |    Error |   StdDev | Ratio | RatioSD |
|------------------- |-------- |----------:|---------:|---------:|------:|--------:|
| WithCircularBuffer |      10 |  28.16 ns | 0.241 ns | 0.188 ns |  1.00 |    0.00 |
|     WithRingBuffer |      10 |  38.79 ns | 0.805 ns | 1.511 ns |  1.32 |    0.04 |
|                    |         |           |          |          |       |         |
| WithCircularBuffer |      50 | 162.60 ns | 1.080 ns | 0.957 ns |  1.00 |    0.00 |
|     WithRingBuffer |      50 | 163.56 ns | 1.887 ns | 1.765 ns |  1.01 |    0.01 |
|                    |         |           |          |          |       |         |
| WithCircularBuffer |     100 | 322.19 ns | 2.993 ns | 2.800 ns |  1.00 |    0.00 |
|     WithRingBuffer |     100 | 319.97 ns | 1.144 ns | 1.070 ns |  0.99 |    0.01 |
```
