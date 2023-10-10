# RingBuffer4chan
Circular buffer, circular queue, cyclic buffer or ring buffer in C#, designed for communication protocols.

## Theory and alternatives

Theory: [Circular buffer](https://en.wikipedia.org/wiki/Circular_buffer) article on Wikipedia.

Alternative: [CircularBuffer-CSharp](https://github.com/joaoportela/CircularBuffer-CSharp) by [João Paulo dos Santos Portela](https://github.com/joaoportela). I copied his unit tests to speed up writing this. Everything is fine, according to MIT License.

Performance results are [here](./src/DeathMatchConsoleApp/).

## The guts
All the guts you need to use are in a single file [RingBuffer.cs](./src/RingBuffer4chan/RingBuffer.cs).