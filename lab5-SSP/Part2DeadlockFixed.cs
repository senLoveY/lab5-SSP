using System;
using System.Threading;
using System.Threading.Tasks;

namespace LabMultiThreading
{
    class Part2DeadlockFixed
    {
        private static object _lockA = new object();
        private static object _lockB = new object();

        public static void Run()
        {
            Console.WriteLine("=== Исправленный Deadlock с таймаутом ===\n");

            Task task1 = Task.Run(() => Thread1());
            Task task2 = Task.Run(() => Thread2());

            Task.WaitAll(task1, task2);

            Console.WriteLine("\nПрограмма успешно завершена (тупик предотвращен). Нажмите Enter...");
            Console.ReadLine();
        }

        static void Thread1()
        {
            if (Monitor.TryEnter(_lockA, TimeSpan.FromSeconds(2)))
            {
                try
                {
                    Console.WriteLine("Thread1: захватил lockA");
                    Thread.Sleep(1000);

                    if (Monitor.TryEnter(_lockB, TimeSpan.FromSeconds(2)))
                    {
                        try { Console.WriteLine("Thread1: захватил lockB"); }
                        finally { Monitor.Exit(_lockB); }
                    }
                    else { Console.WriteLine("!!! Thread1: Не удалось захватить lockB (Таймаут) !!!"); }
                }
                finally { Monitor.Exit(_lockA); }
            }
        }

        static void Thread2()
        {
            // Изменен порядок: Сначала захватываем A (как в Thread1)
            if (Monitor.TryEnter(_lockA, TimeSpan.FromSeconds(2)))
            {
                try
                {
                    Console.WriteLine("Thread2: захватил lockA");
                    Thread.Sleep(1000);

                    if (Monitor.TryEnter(_lockB, TimeSpan.FromSeconds(2)))
                    {
                        try { Console.WriteLine("Thread2: захватил lockB"); }
                        finally { Monitor.Exit(_lockB); }
                    }
                    else { Console.WriteLine("!!! Thread2: Не удалось захватить lockB (Таймаут) !!!"); }
                }
                finally { Monitor.Exit(_lockA); }
            }
            else { Console.WriteLine("!!! Thread2: Не удалось захватить lockA (Таймаут) !!!"); }
        }
    }
}