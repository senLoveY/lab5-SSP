using System;

namespace LabMultiThreading
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Лабораторная работа: Многопоточное программирование ===");
                Console.WriteLine("1. Часть 1: Producer-Consumer (Задания 2, 3 и 4)");
                Console.WriteLine("2. Часть 2: Оригинальный Deadlock");
                Console.WriteLine("3. Часть 2: Исправленный Deadlock");
                Console.WriteLine("4. Часть 3: Межпроцессный Mutex");
                Console.WriteLine("0. Выход");
                Console.Write("\nВыберите пункт меню: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Part1ProducerConsumer.Run();
                        break;
                    case "2":
                        Console.Clear();
                        Part2DeadlockOriginal.Run();
                        break;
                    case "3":
                        Console.Clear();
                        Part2DeadlockFixed.Run();
                        break;
                    case "4":
                        Console.Clear();
                        Part3Mutex.Run();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Нажмите Enter...");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}