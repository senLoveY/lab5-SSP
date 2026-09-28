using System;
using System.Threading;

namespace LabMultiThreading
{
    class Part3Mutex
    {
        public static void Run()
        {
            Console.WriteLine("=== Межпроцессный Мьютекс ===\n");

            using (Mutex mutex = new Mutex(false, "Global\\MyAppMutex"))
            {
                Console.WriteLine("Попытка захвата глобального мьютекса...");
                if (mutex.WaitOne(TimeSpan.FromSeconds(3)))
                {
                    try
                    {
                        Console.WriteLine("Мьютекс захвачен. Программа выполняется.");
                        Console.WriteLine("Внимание: Запустите копию .exe файла и выберите пункт 4.");
                        Console.WriteLine("Нажмите Enter для освобождения мьютекса...");
                        Console.ReadLine();
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                        Console.WriteLine("Мьютекс освобожден.");
                    }
                }
                else
                {
                    Console.WriteLine("\n!!! Не удалось захватить мьютекс (уже запущен другой экземпляр). !!!");
                    Console.WriteLine("Нажмите Enter для возврата...");
                    Console.ReadLine();
                }
            }
        }
    }
}