using System;
using System.Collections.Generic;
using System.IO;

namespace Q4.GradesApp
{
    // a) Student
    public class Student
    {
        public int Id;
        public string FullName = "";
        public int Score;

        public string GetGrade()
        {
            return Score switch
            {
                >= 80 and <= 100 => "A",
                >= 70 and <= 79  => "B",
                >= 60 and <= 69  => "C",
                >= 50 and <= 59  => "D",
                _                => "F"
            };
        }
    }

    // b) & c) Custom exceptions
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // d) Processor
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();

            using var reader = new StreamReader(inputFilePath);
            string? line;
            int lineNo = 0;

            while ((line = reader.ReadLine()) != null)
            {
                lineNo++;
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',', StringSplitOptions.TrimEntries);
                if (parts.Length != 3)
                    throw new MissingFieldException($"Line {lineNo}: Expected 3 fields, got {parts.Length}.");

                if (!int.TryParse(parts[0], out var id))
                    throw new MissingFieldException($"Line {lineNo}: Invalid or missing ID.");

                var fullName = parts[1];

                if (!int.TryParse(parts[2], out var score))
                    throw new InvalidScoreFormatException($"Line {lineNo}: Score '{parts[2]}' is not a valid integer.");

                var s = new Student { Id = id, FullName = fullName, Score = score };
                students.Add(s);
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using var writer = new StreamWriter(outputFilePath);
            foreach (var s in students)
            {
                writer.WriteLine($"{s.FullName} (ID: {s.Id}): Score = {s.Score}, Grade = {s.GetGrade()}");
            }
        }
    }

    // e) Main flow
    public static class Program
    {
        public static void Main()
        {
            try
            {
                var appDir = AppContext.BaseDirectory;
                var input = Path.Combine(appDir, "input.txt");
                var output = Path.Combine(appDir, "report.txt");

                var proc = new StudentResultProcessor();
                var students = proc.ReadStudentsFromFile(input);
                proc.WriteReportToFile(students, output);

                Console.WriteLine("Report written to: " + output);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("[FileNotFound] " + ex.Message);
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine("[InvalidScore] " + ex.Message);
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine("[MissingField] " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[UnknownError] " + ex);
            }
        }
    }
}
