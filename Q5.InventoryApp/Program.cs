using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Q5.InventoryApp
{
    // a) Immutable record + b) Marker interface
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

    // c) Generic Inventory Logger
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private readonly List<T> _log = new();
        private readonly string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item) => _log.Add(item);
        public List<T> GetAll() => new(_log);

        public void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(_log, options);
                using var writer = new StreamWriter(_filePath);
                writer.Write(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[SaveToFile] " + ex.Message);
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("[LoadFromFile] File not found. Starting with empty log.");
                    return;
                }

                using var reader = new StreamReader(_filePath);
                var json = reader.ReadToEnd();
                var list = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

                _log.Clear();
                _log.AddRange(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[LoadFromFile] " + ex.Message);
            }
        }
    }

    // f) Integration Layer
    public class InventoryApp
    {
        private readonly InventoryLogger<InventoryItem> _logger;

        public InventoryApp(string filePath)
        {
            _logger = new InventoryLogger<InventoryItem>(filePath);
        }

        public void SeedSampleData()
        {
            _logger.Add(new InventoryItem(1, "Notebook", 50, DateTime.Now));
            _logger.Add(new InventoryItem(2, "Pen", 200, DateTime.Now));
            _logger.Add(new InventoryItem(3, "Stapler", 20, DateTime.Now));
            _logger.Add(new InventoryItem(4, "Marker", 75, DateTime.Now));
            _logger.Add(new InventoryItem(5, "Envelopes", 300, DateTime.Now));
        }

        public void SaveData() => _logger.SaveToFile();

        public void LoadData() => _logger.LoadFromFile();

        public void PrintAllItems()
        {
            foreach (var item in _logger.GetAll())
                Console.WriteLine($"#{item.Id} {item.Name} — Qty: {item.Quantity}, Added: {item.DateAdded:g}");
        }

        public static void Main()
        {
            var appDir = AppContext.BaseDirectory;
            var dataFile = Path.Combine(appDir, "inventory.json");

            // g) Main flow
            var app = new InventoryApp(dataFile);
            app.SeedSampleData();
            app.SaveData();

            // Simulate new session: new app instance
            var app2 = new InventoryApp(dataFile);
            app2.LoadData();
            Console.WriteLine("Loaded items:");
            app2.PrintAllItems();
        }
    }
}
