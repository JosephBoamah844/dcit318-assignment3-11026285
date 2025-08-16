using System;
using System.Collections.Generic;

namespace Q3.WarehouseApp
{
    // a) Marker interface
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    // b) ElectronicItem
    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public string Brand { get; }
        public int WarrantyMonths { get; }

        public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }

        public override string ToString() => $"Electronics #{Id}: {Name} ({Brand}), Qty={Quantity}, Warranty={WarrantyMonths}m";
    }

    // c) GroceryItem
    public class GroceryItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; }

        public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }

        public override string ToString() => $"Grocery #{Id}: {Name}, Qty={Quantity}, Expires={ExpiryDate:d}";
    }

    // e) Custom exceptions
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message) : base(message) { }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message) { }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

    // d) Generic repository
    public class InventoryRepository<T> where T : IInventoryItem
    {
        private readonly Dictionary<int, T> _items = new();

        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
                throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
            _items[item.Id] = item;
        }

        public T GetItemById(int id)
        {
            if (!_items.TryGetValue(id, out var item))
                throw new ItemNotFoundException($"Item with ID {id} not found.");
            return item;
        }

        public void RemoveItem(int id)
        {
            if (!_items.Remove(id))
                throw new ItemNotFoundException($"Item with ID {id} not found.");
        }

        public List<T> GetAllItems() => new(_items.Values);

        public void UpdateQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0)
                throw new InvalidQuantityException("Quantity cannot be negative.");
            var item = GetItemById(id);
            item.Quantity = newQuantity;
        }
    }

    // f) Manager
    public class WareHouseManager
    {
        private readonly InventoryRepository<ElectronicItem> _electronics = new();
        private readonly InventoryRepository<GroceryItem> _groceries = new();

        public void SeedData()
        {
            _electronics.AddItem(new ElectronicItem(1, "Laptop", 10, "Dell", 24));
            _electronics.AddItem(new ElectronicItem(2, "Headphones", 25, "Sony", 12));
            _electronics.AddItem(new ElectronicItem(3, "Phone", 15, "Samsung", 24));

            _groceries.AddItem(new GroceryItem(1, "Rice (5kg)", 40, DateTime.Today.AddMonths(12)));
            _groceries.AddItem(new GroceryItem(2, "Milk", 20, DateTime.Today.AddDays(14)));
            _groceries.AddItem(new GroceryItem(3, "Eggs (Tray)", 30, DateTime.Today.AddDays(10)));
        }

        public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
        {
            foreach (var item in repo.GetAllItems())
                Console.WriteLine(item);
        }

        public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
        {
            try
            {
                var item = repo.GetItemById(id);
                repo.UpdateQuantity(id, item.Quantity + quantity);
                Console.WriteLine($"Stock increased. #{id} new qty = {item.Quantity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IncreaseStock] {ex.Message}");
            }
        }

        public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
        {
            try
            {
                repo.RemoveItem(id);
                Console.WriteLine($"Item #{id} removed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RemoveItem] {ex.Message}");
            }
        }

        public static void Main()
        {
            var mgr = new WareHouseManager();
            mgr.SeedData();

            Console.WriteLine("== Groceries ==");
            mgr.PrintAllItems(mgr._groceries);

            Console.WriteLine("\n== Electronics ==");
            mgr.PrintAllItems(mgr._electronics);

            Console.WriteLine("\n== Exception Scenarios ==");
            // Duplicate
            try
            {
                mgr._groceries.AddItem(new GroceryItem(1, "Sugar", 5, DateTime.Today.AddMonths(6)));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Duplicate] {ex.Message}");
            }

            // Remove non-existent
            mgr.RemoveItemById(mgr._electronics, 99);

            // Invalid quantity
            try
            {
                mgr._electronics.UpdateQuantity(1, -10);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[InvalidQty] {ex.Message}");
            }
        }
    }
}
