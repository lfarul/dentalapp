using Dental_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Dental_App.DataSeed
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(

                new Employee { EmployeeID = 1, FirstName = "Konrad", LastName = "Kowalski", Specialty = "Stomatolog", Email = "kkowalski@klinika" },
                new Employee { EmployeeID = 2, FirstName = "Agata", LastName = "Nowak", Specialty = "Ortodonta", Email = "anowak@klinika" },
                new Employee { EmployeeID = 3, FirstName = "Jan", LastName = "Kujawski", Specialty = "Chirurg szczękowy", Email = "jkujawski@klinika" },
                new Employee { EmployeeID = 4, FirstName = "Damian", LastName = "Borowicz", Specialty = "Stomatolog", Email = "dborowicz@klinika" },
                new Employee { EmployeeID = 5, FirstName = "Karolina", LastName = "Zawadzka", Specialty = "Stomatolog", Email = "kzawadzka@klinika" }
            );

        }
    }
}
