using System;
using System.Threading;
using System.Threading.Tasks;

namespace LabMultiThreading
{
    class Part2DeadlockOriginal
    {
        private static object _lockA = new object();
        private static object _lockB = new object();

        public static void Run()
        {
            Console.WriteLine("=== Демонстрация Deadlock (Оригинал) ===\n");

            Task task1 = Task.Run(() => Thread1());
            Task task2 = Task.Run(() => Thread2());

            Console.WriteLine("Ожидание завершения потоков... (ПРОГРАММА ЗАВИСНЕТ)");
            Console.WriteLine("-> Нажмите ПАУЗУ (Break All) в Visual Studio и откройте Debug -> Windows -> Parallel Stacks!");

            Task.WaitAll(task1, task2);

            Console.WriteLine("Программа завершена (это сообщение не будет выведено)");
        }

        static void Thread1()
        {
            lock (_lockA)
            {
                Console.WriteLine("Thread1: захватил lockA");
                Thread.Sleep(1000);
                lock (_lockB)
                {
                    Console.WriteLine("Thread1: захватил lockB");
                }
            }
        }

        static void Thread2()
        {
            lock (_lockB)
            {
                Console.WriteLine("Thread2: захватил lockB");
                Thread.Sleep(1000);
                lock (_lockA)
                {
                    Console.WriteLine("Thread2: захватил lockA");
                }
            }
        }
    }
}