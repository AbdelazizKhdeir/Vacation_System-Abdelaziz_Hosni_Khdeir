using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Vacation_System.Data;
using Vacation_System.Entity;
using Vacation_System.Services;

namespace EmployeeVacationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure database connection
            const string connectionString = "Server=server_name;Database=DB_name;Trusted_Connection=True;";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            using var context = new AppDbContext(optionsBuilder.Options);

            try
            {
                // Initialize database and seed data
                context.Database.EnsureCreated();
                SeedData.Initialize(context);
                Console.WriteLine("Database initialized successfully.\n");

                // Initialize services
                var employeeService = new EmployeeService(context);
                var vacationService = new VacationRequestService(context);
                var reportService = new ReportService(context);

                DisplayMainMenu();

                while (true)
                {
                    Console.Write("\nEnter your choice (1-6, 0 to exit): ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            ShowAllEmployees(context);
                            break;
                        case "2":
                            UpdateEmployeeDemo(employeeService);
                            break;
                        case "3":
                            SubmitVacationRequestDemo(vacationService);
                            break;
                        case "4":
                            ProcessPendingRequests(context, vacationService, employeeService);
                            break;
                        case "5":
                            ShowEmployeeReport(context);
                            break;
                        case "6":
                            ShowVacationHistory(context);
                            break;
                        case "0":
                            Console.WriteLine("Exiting system...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("==== Employee Vacation System ====");
            Console.WriteLine("1. List All Employees");
            Console.WriteLine("2. Update Employee Information");
            Console.WriteLine("3. Submit Vacation Request");
            Console.WriteLine("4. Process Pending Requests");
            Console.WriteLine("5. View Employee Report");
            Console.WriteLine("6. View Vacation History");
            Console.WriteLine("0. Exit");
        }

        static void ShowAllEmployees(AppDbContext context)
        {
            Console.WriteLine("\n=== All Employees ===");
            var employees = context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Select(e => new {
                    e.EmployeeNumber,
                    e.EmployeeName,
                    Department = e.Department.DepartmentName,
                    Position = e.Position.PositionName,
                    e.Salary
                }).ToList();

            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.EmployeeNumber} | {emp.EmployeeName} | {emp.Department} | {emp.Position} | {emp.Salary:C}");
            }
        }

        static void UpdateEmployeeDemo(EmployeeService service)
        {
            Console.WriteLine("\n=== Update Employee ===");
            Console.Write("Enter employee number: ");
            var empNumber = Console.ReadLine();

            Console.Write("New name (press enter to skip): ");
            var name = Console.ReadLine();

            Console.Write("New salary (press enter to skip): ");
            var salaryInput = Console.ReadLine();

            service.UpdateEmployee(empNumber, emp =>
            {
                if (!string.IsNullOrEmpty(name)) emp.EmployeeName = name;
                if (decimal.TryParse(salaryInput, out var salary)) emp.Salary = salary;
            });

            Console.WriteLine("Employee updated successfully.");
        }

        static void SubmitVacationRequestDemo(VacationRequestService service)
        {
            Console.WriteLine("\n=== New Vacation Request ===");

            var request = new VacationRequest
            {
                RequestSubmissionDate = DateTime.Now,
                Description = GetInput("Description: "),
                EmployeeNumber = GetInput("Employee Number: "),
                VacationTypeCode = GetInput("Vacation Type (S/U/A/O/B): ").ToUpper(),
                StartDate = DateTime.Parse(GetInput("Start Date (yyyy-mm-dd): ")),
                EndDate = DateTime.Parse(GetInput("End Date (yyyy-mm-dd): ")),
                TotalVacationDays = int.Parse(GetInput("Total Days: ")),
                RequestStateId = 1
            };

            var result = service.SubmitRequest(request);
            Console.WriteLine(result ? "Request submitted successfully!" : "Submission failed - date conflict!");
        }

        static void ProcessPendingRequests(AppDbContext context, VacationRequestService vacService, EmployeeService empService)
        {
            Console.WriteLine("\n=== Pending Requests ===");
            var pending = context.VacationRequests
                .Include(vr => vr.Employee)
                .Where(vr => vr.RequestStateId == 1)
                .ToList();

            foreach (var req in pending)
            {
                Console.WriteLine($"\nRequest ID: {req.RequestId}");
                Console.WriteLine($"Employee: {req.Employee.EmployeeName} ({req.EmployeeNumber})");
                Console.WriteLine($"Dates: {req.StartDate:d} to {req.EndDate:d} ({req.TotalVacationDays} days)");
                Console.Write("Approve (A) or Decline (D): ");

                var decision = Console.ReadLine().ToUpper();
                if (decision == "A")
                {
                    vacService.ApproveRequest(req.RequestId, req.Employee.ReportedToEmployeeNumber);
                    empService.UpdateVacationDays(req.EmployeeNumber, req.TotalVacationDays);
                    Console.WriteLine("Request approved!");
                }
                else
                {
                    vacService.DeclinedByEmployeeNumber(req.RequestId, req.Employee.ReportedToEmployeeNumber);
                    Console.WriteLine("Request declined!");
                }
            }
        }

        static void ShowEmployeeReport(AppDbContext context)
        {
            Console.Write("\nEnter employee number: ");
            var empNumber = Console.ReadLine();

            var employee = context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.ReportedToEmployeeNumber)  // Fixed include
                .FirstOrDefault(e => e.EmployeeNumber == empNumber);

            Console.WriteLine($"\n=== Employee Report ({employee.EmployeeNumber}) ===");
            Console.WriteLine($"Name: {employee.EmployeeName}");
            Console.WriteLine($"Department: {employee.Department.DepartmentName}");
            Console.WriteLine($"Position: {employee.Position.PositionName}");
            Console.WriteLine($"Manager: {employee.ReportedToEmployeeNumber?.EmployeeName ?? "None"}");  // Fixed access
            Console.WriteLine($"Vacation Days Left: {employee.VacationDaysLeft}");
            Console.WriteLine($"Salary: {employee.Salary:C}");
        }

        static void ShowVacationHistory(AppDbContext context)
        {
            Console.Write("\nEnter employee number: ");
            var empNumber = Console.ReadLine();

            var history = context.VacationRequests
                .Include(vr => vr.VacationType)
                .Include(vr => vr.ApprovedByEmployeeNumber)  // Fixed include
                .Where(vr => vr.EmployeeNumber == empNumber && vr.RequestStateId == 2)
                .ToList();

            Console.WriteLine($"\n=== Vacation History ({empNumber}) ===");
            foreach (var req in history)
            {
                Console.WriteLine($"{req.VacationType.Name} | {req.Description}");
                Console.WriteLine($"{req.StartDate:d} - {req.EndDate:d} ({req.TotalVacationDays} days)");
                Console.WriteLine($"Approved by: {req.ApprovedByEmployeeNumber?.EmployeeName ?? "N/A"}");  // Fixed access
            }
        }
        

        static string GetInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
