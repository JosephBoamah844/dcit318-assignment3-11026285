# DCIT 318 – Programming II  
## Assignment 3 — Complete .NET Solution (Q1–Q5)

This solution contains five C# console apps, one per question. It demonstrates:  
- Records, interfaces, inheritance, sealed classes (Q1)  
- Collections, generics, grouping with dictionaries (Q2)  
- Generics with **custom exceptions** (Q3)  
- File I/O, validation, **custom exceptions** (Q4)  
- Records, generics, JSON persistence (Q5)  

---

## Quick Start

### Requirements
- .NET 8 SDK (check with `dotnet --version`)  
- Any IDE: VS Code, Visual Studio 2022, Rider  

### Build & Run
```bash
dotnet build
dotnet run --project Q1.FinanceApp
dotnet run --project Q2.HealthSystemApp
dotnet run --project Q3.WarehouseApp
dotnet run --project Q4.GradesApp
dotnet run --project Q5.InventoryApp
````

---

## Project Layout

```
DCIT318_Assignment3.sln
Q1.FinanceApp/Program.cs
Q2.HealthSystemApp/Program.cs
Q3.WarehouseApp/Program.cs
Q4.GradesApp/
  Program.cs
  input.txt       # needed for Q4
Q5.InventoryApp/Program.cs
```

---

## Question Breakdown

### Q1 — Finance Management

* `record Transaction(...)`
* `ITransactionProcessor` (3 implementations)
* `Account` base class, `SavingsAccount` sealed subclass
* Demo shows transactions processed + balance updates

### Q2 — Healthcare System

* Generic `Repository<T>`
* Entities: Patient, Prescription
* Dictionary grouping by PatientId
* Demo prints all patients + prescriptions per patient

### Q3 — Warehouse Inventory

* Interface `IInventoryItem`
* Generics with constraints
* Custom exceptions: `DuplicateItemException`, `ItemNotFoundException`, `InvalidQuantityException`
* Demo shows normal use + error handling

### Q4 — Grades Processing

* Input file: `input.txt`
* Validates records, throws custom exceptions for bad data
* Generates `report.txt` with names, scores, grades

### Q5 — Inventory with JSON

* `record InventoryItem(...)`
* Generic `InventoryLogger<T>`
* Save/load to `inventory.json` with JSON serialization
* Demo seeds data, saves, reloads, prints

---

## Testing Checklist

* `dotnet build` compiles with no errors
* Each project runs independently with expected output
* Q4:

  * Invalid input → error message
  * Valid input → generates `report.txt`
* Q5: creates and reloads `inventory.json`

---
