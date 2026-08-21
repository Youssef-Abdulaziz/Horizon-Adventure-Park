using Horizon_Adventure_Park.Enums;

namespace Horizon_Adventure_Park.Models
{
    public class Employee
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public EmployeeRole Role { get; set; }
        public bool IsAvailable { get; set; }

        public Employee(
            string employeeId,
            string name,
            EmployeeRole role)
        {
            EmployeeId = employeeId;
            Name = name;
            Role = role;
            IsAvailable = true;
        }

        public void SetAvailability(bool available)
        {
            IsAvailable = available;
        }
    }
}