using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation_System.Entity;

namespace Vacation_System.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Seed Vacation Types
            if (!context.VacationTypes.Any())
            {
                context.VacationTypes.AddRange(new[]
                {
                new VacationType { Code = "S", Name = "Sick" },
                new VacationType { Code = "U", Name = "Unpaid" },
                new VacationType { Code = "A", Name = "Annual" },
                new VacationType { Code = "O", Name = "Day Off" },
                new VacationType { Code = "B", Name = "Business Trip" }
            });
                context.SaveChanges();
            }

            // Seed Request States
            if (!context.RequestStates.Any())
            {
                context.RequestStates.AddRange(new[]
                {
                new RequestState { StateId = 1, Name = "Submitted" },
                new RequestState { StateId = 2, Name = "Approved" },
                new RequestState { StateId = 3, Name = "Declined" }
            });
                context.SaveChanges();
            }

            // Seed Departments
            if (!context.Departments.Any())
            {
                var departments = Enumerable.Range(1, 20)
                    .Select(i => new Department { DepartmentName = $"Department {i}" });
                context.Departments.AddRange(departments);
                context.SaveChanges();
            }

            // Seed Positions
            if (!context.Positions.Any())
            {
                var positions = Enumerable.Range(1, 20)
                    .Select(i => new Position { PositionName = $"Position {i}" });
                context.Positions.AddRange(positions);
                context.SaveChanges();
            }

            // Seed Employees
            if (!context.Employees.Any())
            {
                var employees = new List<Employee>();
                var random = new Random();

                for (int i = 1; i <= 10; i++)
                {
                    employees.Add(new Employee(
                        number: $"E{i:00000}",
                        name: $"Employee {i}")
                    {
                        DepartmentId = random.Next(1, 21),
                        PositionId = random.Next(1, 21),
                        GenderCode = i % 2 == 0 ? "M" : "F",
                        ReportedToEmployeeNumber = i > 1 ? "E00001" : null,
                       // Salary = Math.Round(3000 + (i * 100) + random.Next(0, 500), 2),
                        VacationDaysLeft = 24
                    });
                }
                context.Employees.AddRange(employees);
                context.SaveChanges();
            }

            // Seed Sample Vacation Requests
            if (!context.VacationRequests.Any())
            {
                var requests = new[]
                {
                new VacationRequest
                {
                    EmployeeNumber = "E00002",
                    RequestSubmissionDate = DateTime.Now.AddDays(-5),
                    Description = "Family vacation",
                    VacationTypeCode = "A",
                    StartDate = DateTime.Today.AddDays(10),
                    EndDate = DateTime.Today.AddDays(15),
                    TotalVacationDays = 5,
                    RequestStateId = 2,
                    ApprovedByEmployeeNumber = "E00001"
                },
                new VacationRequest
                {
                    EmployeeNumber = "E00003",
                    RequestSubmissionDate = DateTime.Now.AddDays(-2),
                    Description = "Medical leave",
                    VacationTypeCode = "S",
                    StartDate = DateTime.Today.AddDays(3),
                    EndDate = DateTime.Today.AddDays(5),
                    TotalVacationDays = 2,
                    RequestStateId = 1
                }
            };
                context.VacationRequests.AddRange(requests);
                context.SaveChanges();
            }
        }
    }

}

