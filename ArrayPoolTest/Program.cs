using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

BenchmarkRunner.Run<Tests>();

[MemoryDiagnoser]
public class Tests {
    int size = 1000000;
    
    [Benchmark]
    public void TestArrayPool()
    {
        var pool = System.Buffers.ArrayPool<int>.Create();
        var array = pool.Rent(size);
        for (int i = 0; i < size; i++)
            array[i] = i;
        pool.Return(array, true);
    }

    [Benchmark]
    public void TestList()
    {
        var list = new List<int>(size);
        for (int i = 0; i < size; i++)
            list.Add(i);
    }
}