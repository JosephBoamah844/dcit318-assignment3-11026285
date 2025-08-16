using System;
using System.Collections.Generic;
using System.Linq;

namespace Q2.HealthSystemApp
{
    // a) Generic repository
    public class Repository<T>
    {
        private readonly List<T> items = new();

        public void Add(T item) => items.Add(item);
        public List<T> GetAll() => new(items);

        public T? GetById(Func<T, bool> predicate) => items.FirstOrDefault(predicate);

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item is null) return false;
            items.Remove(item);
            return true;
        }
    }

    // b) Patient
    public class Patient
    {
        public int Id;
        public string Name;
        public int Age;
        public string Gender;

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString() => $"Patient #{Id}: {Name}, {Age}, {Gender}";
    }

    // c) Prescription
    public class Prescription
    {
        public int Id;
        public int PatientId;
        public string MedicationName;
        public DateTime DateIssued;

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString() => $"Rx #{Id} for Patient {PatientId}: {MedicationName} ({DateIssued:d})";
    }

    public class HealthSystemApp
    {
        private readonly Repository<Patient> _patientRepo = new();
        private readonly Repository<Prescription> _prescriptionRepo = new();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new();

        public void SeedData()
        {
            // Patients
            _patientRepo.Add(new Patient(1, "Alice Smith", 30, "Female"));
            _patientRepo.Add(new Patient(2, "Kwame Mensah", 45, "Male"));
            _patientRepo.Add(new Patient(3, "Abena Owusu", 29, "Female"));

            // Prescriptions
            _prescriptionRepo.Add(new Prescription(1, 1, "Amoxicillin", DateTime.Today.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(2, 1, "Ibuprofen", DateTime.Today.AddDays(-6)));
            _prescriptionRepo.Add(new Prescription(3, 2, "Metformin", DateTime.Today.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(4, 3, "Vitamin D", DateTime.Today.AddDays(-1)));
            _prescriptionRepo.Add(new Prescription(5, 2, "Atorvastatin", DateTime.Today));
        }

        public void BuildPrescriptionMap()
        {
            _prescriptionMap = _prescriptionRepo
                .GetAll()
                .GroupBy(p => p.PatientId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        public void PrintAllPatients()
        {
            Console.WriteLine("All Patients:");
            foreach (var p in _patientRepo.GetAll())
                Console.WriteLine(p);
        }

        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            return _prescriptionMap.TryGetValue(patientId, out var list) ? list : new List<Prescription>();
        }

        public void PrintPrescriptionsForPatient(int id)
        {
            var list = GetPrescriptionsByPatientId(id);
            if (list.Count == 0)
            {
                Console.WriteLine($"No prescriptions found for patient {id}.");
                return;
            }

            Console.WriteLine($"Prescriptions for patient {id}:");
            foreach (var rx in list)
                Console.WriteLine(rx);
        }

        public static void Main()
        {
            var app = new HealthSystemApp();
            app.SeedData();
            app.BuildPrescriptionMap();
            app.PrintAllPatients();
            Console.WriteLine();

            // Pick one patient and print
            app.PrintPrescriptionsForPatient(2);
        }
    }
}
