using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LabMultiThreading
{
    class Part1ProducerConsumer
    {
        // Задание 4: Добавлен класс DataItem
        class DataItem
        {
            public int Id;
            public string Payload;
        }

        // Задание 4: Очередь теперь хранит DataItem
        private static Queue<DataItem> _buffer = new Queue<DataItem>();

        // Задание 2: Размер буфера
        private const int BufferSize = 5;

        private static SemaphoreSlim _emptySlots = new SemaphoreSlim(BufferSize, BufferSize);
        private static SemaphoreSlim _filledSlots = new SemaphoreSlim(0, BufferSize);
        private static Mutex _mutex = new Mutex();
        private static bool _isRunning = true;

        public static void Run()
        {
            Console.WriteLine("=== Producer-Consumer (Задания 3 и 4) ===\n");
            _isRunning = true;

            // Задание 3: 3 производителя
            Task producer1 = Task.Run(() => Producer(1));
            Task producer2 = Task.Run(() => Producer(2));
            Task producer3 = Task.Run(() => Producer(3));

            // Задание 3: 4 потребителя
            Task consumer1 = Task.Run(() => Consumer(1));
            Task consumer2 = Task.Run(() => Consumer(2));
            Task consumer3 = Task.Run(() => Consumer(3));
            Task consumer4 = Task.Run(() => Consumer(4));

            // Работаем 5 секунд
            Thread.Sleep(5000);
            _isRunning = false;

            Task.WaitAll(producer1, producer2, producer3, consumer1, consumer2, consumer3, consumer4);

            Console.WriteLine("\nВсе потоки завершены. Нажмите Enter для возврата в меню...");
            Console.ReadLine();
        }

        static async Task Producer(int id)
        {
            Random rand = new Random(id);
            int dataIdCounter = 1;
            while (_isRunning)
            {
                await _emptySlots.WaitAsync();
                _mutex.WaitOne();
                try
                {
                    var item = new DataItem { Id = dataIdCounter++, Payload = $"Info_From_Pr_{id}" };
                    _buffer.Enqueue(item);
                    Console.WriteLine($"[Producer {id}] Добавлен: {item.Payload} (Id:{item.Id}). В буфере: {_buffer.Count}");
                }
                finally
                {
                    _mutex.ReleaseMutex();
                }
                _filledSlots.Release();
                await Task.Delay(rand.Next(200, 600));
            }
        }

        static async Task Consumer(int id)
        {
            Random rand = new Random(id + 10);
            while (_isRunning)
            {
                await _filledSlots.WaitAsync();
                _mutex.WaitOne();
                try
                {
                    DataItem item = _buffer.Dequeue();
                    Console.WriteLine($"[Consumer {id}] Извлечен: {item.Payload} (Id:{item.Id}). В буфере: {_buffer.Count}");
                }
                finally
                {
                    _mutex.ReleaseMutex();
                }
                _emptySlots.Release();
                await Task.Delay(rand.Next(300, 800));
            }
        }
    }
}