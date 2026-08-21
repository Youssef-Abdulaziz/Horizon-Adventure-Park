namespace Horizon_Adventure_Park.Models
{
    public class StaffAssignment
    {
        public string AssignmentId { get; set; }
        public string EmployeeId { get; set; }
        public string RideId { get; set; }
        public string TimePeriod { get; set; }

        public StaffAssignment(
            string assignmentId,
            string employeeId,
            string rideId,
            string timePeriod)
        {
            AssignmentId = assignmentId;
            EmployeeId = employeeId;
            RideId = rideId;
            TimePeriod = timePeriod;
        }
    }
}