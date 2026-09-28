using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumerLab
{
    class DataItem
    {
        public int Id { get; set; }
        public string Payload { get; set; }
    }

    class Program
    {
        // Измененная очередь для хранения объектов DataItem
        private static Queue<DataItem> _buffer = new Queue<DataItem>();
        private const int BufferSize = 5;

        private static SemaphoreSlim _emptySlots = new SemaphoreSlim(BufferSize, BufferSize);
        private static SemaphoreSlim _filledSlots = new SemaphoreSlim(0, BufferSize);
        private static Mutex _mutex = new Mutex();
        private static bool _isRunning = true;

        static void Main(string[] args)
        {
            Console.WriteLine("=== Producer-Consumer (Tasks 3 & 4) ===\n");

            Task p1 = Task.Run(() => Producer(1));
            Task p2 = Task.Run(() => Producer(2));
            Task p3 = Task.Run(() => Producer(3));

            Task c1 = Task.Run(() => Consumer(1));
            Task c2 = Task.Run(() => Consumer(2));
            Task c3 = Task.Run(() => Consumer(3));
            Task c4 = Task.Run(() => Consumer(4));

            Thread.Sleep(5000);
            _isRunning = false;

            Task.WaitAll(p1, p2, p3, c1, c2, c3, c4);

            Console.WriteLine("\nВсе потоки завершены. Нажмите любую клавишу...");
            Console.ReadKey();
        }

        static async Task Producer(int id)
        {
            Random rand = new Random(id);
            int itemCounter = 1;
            while (_isRunning)
            {
                await _emptySlots.WaitAsync();
                _mutex.WaitOne();
                try
                {
                    var data = new DataItem
                    {
                        Id = itemCounter++,
                        Payload = $"Data from P{id}"
                    };

                    _buffer.Enqueue(data);
                    Console.WriteLine($"[Producer {id}] Добавлен: {data.Payload} (Id:{data.Id}). В буфере: {_buffer.Count}");
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
                DataItem item = null;
                try
                {
                    item = _buffer.Dequeue();
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