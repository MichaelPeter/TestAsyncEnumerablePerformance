using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TestAsyncEnumerablePerformance
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecuteTests(1000);
            ExecuteTests(100 * 1000);
            ExecuteTests(1000 * 1000);
            ExecuteTests(10 * 1000 * 1000);

            Console.WriteLine("Test Finished");
            Console.ReadLine();

            /* Results in Release on Ryzen 3900
             * 
                Operation LoopSync Iterations: 1000 took 0 milliseconds. Result 500500
                Operation LoopAsync Iterations 1000 took 51 milliseconds. Result 500500
                Operation LoopSync Iterations: 1000 took 0 milliseconds. Result 500500
                Operation LoopAsync Iterations 1000 took 0 milliseconds. Result 500500
                Operation LoopSync Iterations: 1000 took 0 milliseconds. Result 500500
                Operation LoopAsync Iterations 1000 took 0 milliseconds. Result 500500
                Operation LoopSync Iterations: 1000 took 0 milliseconds. Result 500500
                Operation LoopAsync Iterations 1000 took 0 milliseconds. Result 500500
                Operation LoopSync Iterations: 1000 took 0 milliseconds. Result 500500
                Operation LoopAsync Iterations 1000 took 0 milliseconds. Result 500500
                Operation LoopSync Iterations: 100000 took 0 milliseconds. Result 5000050000
                Operation LoopAsync Iterations 100000 took 16 milliseconds. Result 5000050000
                Operation LoopSync Iterations: 100000 took 0 milliseconds. Result 5000050000
                Operation LoopAsync Iterations 100000 took 16 milliseconds. Result 5000050000
                Operation LoopSync Iterations: 100000 took 0 milliseconds. Result 5000050000
                Operation LoopAsync Iterations 100000 took 17 milliseconds. Result 5000050000
                Operation LoopSync Iterations: 100000 took 0 milliseconds. Result 5000050000
                Operation LoopAsync Iterations 100000 took 17 milliseconds. Result 5000050000
                Operation LoopSync Iterations: 100000 took 0 milliseconds. Result 5000050000
                Operation LoopAsync Iterations 100000 took 17 milliseconds. Result 5000050000
                Operation LoopSync Iterations: 1000000 took 7 milliseconds. Result 500000500000
                Operation LoopAsync Iterations 1000000 took 128 milliseconds. Result 500000500000
                Operation LoopSync Iterations: 1000000 took 7 milliseconds. Result 500000500000
                Operation LoopAsync Iterations 1000000 took 122 milliseconds. Result 500000500000
                Operation LoopSync Iterations: 1000000 took 10 milliseconds. Result 500000500000
                Operation LoopAsync Iterations 1000000 took 138 milliseconds. Result 500000500000
                Operation LoopSync Iterations: 1000000 took 7 milliseconds. Result 500000500000
                Operation LoopAsync Iterations 1000000 took 105 milliseconds. Result 500000500000
                Operation LoopSync Iterations: 1000000 took 7 milliseconds. Result 500000500000
                Operation LoopAsync Iterations 1000000 took 99 milliseconds. Result 500000500000
                Operation LoopSync Iterations: 10000000 took 74 milliseconds. Result 50000005000000
                Operation LoopAsync Iterations 10000000 took 1061 milliseconds. Result 50000005000000
                Operation LoopSync Iterations: 10000000 took 73 milliseconds. Result 50000005000000
                Operation LoopAsync Iterations 10000000 took 1101 milliseconds. Result 50000005000000
                Operation LoopSync Iterations: 10000000 took 73 milliseconds. Result 50000005000000
                Operation LoopAsync Iterations 10000000 took 1004 milliseconds. Result 50000005000000
                Operation LoopSync Iterations: 10000000 took 73 milliseconds. Result 50000005000000
                Operation LoopAsync Iterations 10000000 took 1023 milliseconds. Result 50000005000000
                Operation LoopSync Iterations: 10000000 took 76 milliseconds. Result 50000005000000
                Operation LoopAsync Iterations 10000000 took 1017 milliseconds. Result 50000005000000
             
             */
        }

        private static void ExecuteTests(int iterations)
        {
            for (int i = 0; i < 5; i++)
            {
                Stopwatch($"LoopSync Iterations: {iterations}", () => ExecuteLoopSync(iterations));
                Stopwatch($"LoopAsync Iterations {iterations}", () => ExecuteLoopAsync(iterations).Result);
            }
        }

        private static void Stopwatch(string opDescription, Func<long> a)
        {
            var sw = new Stopwatch();
            sw.Start();
            long result = a();
            sw.Stop();
            Console.WriteLine($"Operation {opDescription} took {sw.ElapsedMilliseconds} milliseconds. Result {result}");
        }

        private static long ExecuteLoopSync(long iterations)
        {
            long total = 0;
            foreach (long num in GetSyncSource(iterations))
            {
                total += num;
            }
            return total;
        }

        private static async Task<long> ExecuteLoopAsync(long iterations)
        {
            long total = 0;
            await foreach (long num in GetAsyncSource(iterations))
            {
                total += num;
            }
            return total;
        }

        private static IAsyncEnumerable<int> GetAsyncSource(long iterations)
        {
            return GetSyncSource(iterations).ToAsyncEnumerable();
        }

        private static IEnumerable<int> GetSyncSource(long iterations)
        {
            int counter = 0;
            while (counter < iterations)
                yield return ++counter;
        }
    }
}
