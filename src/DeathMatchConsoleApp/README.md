# 2022-12-04 at @[6c232032](https://github.com/r-pankevicius/RingBuffer4chan/commit/6c23203212ab86bc37b80e12d3a644ed3d58bd24)

## AddOne
|             Method | Times |     Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------- |------ |---------:|----------:|----------:|------:|--------:|
| **WithCircularBuffer** |    **10** | **1.524 ns** | **0.0215 ns** | **0.0191 ns** |  **1.00** |    **0.00** |
|     WithRingBuffer |    10 | 1.958 ns | 0.0267 ns | 0.0237 ns |  1.28 |    0.02 |
|                    |       |          |           |           |       |         |
| **WithCircularBuffer** |   **100** | **1.354 ns** | **0.0235 ns** | **0.0208 ns** |  **1.00** |    **0.00** |
|     WithRingBuffer |   100 | 1.948 ns | 0.0179 ns | 0.0158 ns |  1.44 |    0.02 |
|                    |       |          |           |           |       |         |
| **WithCircularBuffer** |  **1000** | **1.828 ns** | **0.0068 ns** | **0.0064 ns** |  **1.00** |    **0.00** |
|     WithRingBuffer |  1000 | 2.412 ns | 0.0027 ns | 0.0024 ns |  1.32 |    0.01 |

## AddOneTakeOne
|             Method | Times |     Mean |     Error |    StdDev | Ratio | RatioSD |
|------------------- |------ |---------:|----------:|----------:|------:|--------:|
| **WithCircularBuffer** |    **10** | **2.389 ns** | **0.0162 ns** | **0.0135 ns** |  **1.00** |    **0.00** |
|     WithRingBuffer |    10 | 4.530 ns | 0.0318 ns | 0.0297 ns |  1.90 |    0.02 |
|                    |       |          |           |           |       |         |
| **WithCircularBuffer** |   **100** | **2.617 ns** | **0.0306 ns** | **0.0286 ns** |  **1.00** |    **0.00** |
|     WithRingBuffer |   100 | 3.894 ns | 0.0326 ns | 0.0255 ns |  1.49 |    0.02 |
|                    |       |          |           |           |       |         |
| **WithCircularBuffer** |  **1000** | **2.632 ns** | **0.0403 ns** | **0.0377 ns** |  **1.00** |    **0.00** |
|     WithRingBuffer |  1000 | 4.505 ns | 0.0216 ns | 0.0202 ns |  1.71 |    0.03 |```

## AddMultiple
|             Method | HowMany |       Mean |     Error |    StdDev | Ratio |
|------------------- |-------- |-----------:|----------:|----------:|------:|
| **WithCircularBuffer** |      **10** |  **18.192 ns** | **0.1288 ns** | **0.1205 ns** |  **1.00** |
|     WithRingBuffer |      10 |   5.431 ns | 0.0308 ns | 0.0241 ns |  0.30 |
|                    |         |            |           |           |       |
| **WithCircularBuffer** |      **50** |  **89.876 ns** | **1.1430 ns** | **0.9545 ns** |  **1.00** |
|     WithRingBuffer |      50 |  10.021 ns | 0.0802 ns | 0.0711 ns |  0.11 |
|                    |         |            |           |           |       |
| **WithCircularBuffer** |     **100** | **197.052 ns** | **3.9391 ns** | **4.9816 ns** |  **1.00** |
|     WithRingBuffer |     100 |  11.528 ns | 0.0554 ns | 0.0518 ns |  0.06 |

## AddMultipleTakeMultiple
|             Method | HowMany |      Mean |    Error |   StdDev | Ratio |
|------------------- |-------- |----------:|---------:|---------:|------:|
| **WithCircularBuffer** |      **10** |  **30.72 ns** | **0.287 ns** | **0.254 ns** |  **1.00** |
|     WithRingBuffer |      10 |  16.12 ns | 0.291 ns | 0.272 ns |  0.52 |
|                    |         |           |          |          |       |
| **WithCircularBuffer** |      **50** | **160.91 ns** | **1.281 ns** | **1.198 ns** |  **1.00** |
|     WithRingBuffer |      50 |  25.32 ns | 0.173 ns | 0.162 ns |  0.16 |
|                    |         |           |          |          |       |
| **WithCircularBuffer** |     **100** | **300.15 ns** | **2.564 ns** | **2.399 ns** |  **1.00** |
|     WithRingBuffer |     100 |  32.18 ns | 0.337 ns | 0.315 ns |  0.11 |
