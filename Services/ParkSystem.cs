using Horizon_Adventure_Park.Enums;
using Horizon_Adventure_Park.Models;


namespace Horizon_Adventure_Park.Services
{
    public class ThemeParkSystem
    {
        private readonly List<Visitor> visitors = new();
        private readonly List<Ticket> tickets = new();
        private readonly List<Ride> rides = new();
        private readonly List<RideReservation> reservations = new();
        private readonly List<Employee> employees = new();
        private readonly List<StaffAssignment> assignments = new();

        // Visitor operations
        public bool RegisterVisitor(Visitor visitor)
        {
            if (visitors.Any(v => v.VisitorId == visitor.VisitorId))
            {
                Console.WriteLine("Registration failed: Visitor ID already exists.");
                return false;
            }

            visitors.Add(visitor);

            Console.WriteLine("Visitor registered successfully.");
            return true;
        }

        public Visitor? FindVisitor(string visitorId)
        {
            return visitors.FirstOrDefault(v => v.VisitorId == visitorId);
        }

        // Ticket operations
        public bool IssueTicket(Ticket ticket)
        {
            Visitor? visitor = FindVisitor(ticket.VisitorId);

            if (visitor == null)
            {
                Console.WriteLine("Ticket issuance failed: Visitor does not exist.");
                return false;
            }

            if (tickets.Any(t =>
                t.VisitorId == ticket.VisitorId &&
                t.Status == TicketStatus.Active))
            {
                Console.WriteLine("Visitor already has an active ticket.");
                return false;
            }

            tickets.Add(ticket);

            Console.WriteLine("Ticket issued successfully.");
            return true;
        }

        public Ticket? FindActiveTicket(string visitorId)
        {
            return tickets.FirstOrDefault(t =>
                t.VisitorId == visitorId &&
                t.IsValid());
        }

        // Ride operations
        public void AddRide(Ride ride)
        {
            if (rides.Any(r => r.RideId == ride.RideId))
            {
                Console.WriteLine("Ride ID already exists.");
                return;
            }

            rides.Add(ride);
            Console.WriteLine("Ride added successfully.");
        }

        public Ride? FindRide(string rideId)
        {
            return rides.FirstOrDefault(r => r.RideId == rideId);
        }

        public bool ValidateRideAccess(
            string visitorId,
            string rideId)
        {
            Visitor? visitor = FindVisitor(visitorId);

            if (visitor == null)
            {
                Console.WriteLine("ACCESS DENIED");
                Console.WriteLine("Reason: Visitor does not exist.");
                return false;
            }

            Ride? ride = FindRide(rideId);

            if (ride == null)
            {
                Console.WriteLine("ACCESS DENIED");
                Console.WriteLine("Reason: Ride does not exist.");
                return false;
            }

            Ticket? ticket = FindActiveTicket(visitorId);

            if (ticket == null)
            {
                Console.WriteLine("ACCESS DENIED");
                Console.WriteLine("Reason: Visitor does not have a valid ticket.");
                return false;
            }

            string result = ride.CheckEligibility(visitor, ticket);

            if (result != "Eligible")
            {
                Console.WriteLine("ACCESS DENIED");
                Console.WriteLine($"Reason: {result}");
                return false;
            }

            Console.WriteLine("ACCESS GRANTED");

            ride.AdmitVisitor();

            return true;
        }

        public void UpdateRideStatus(
            string rideId,
            RideStatus newStatus)
        {
            Ride? ride = FindRide(rideId);

            if (ride == null)
            {
                Console.WriteLine("Ride does not exist.");
                return;
            }

            ride.UpdateStatus(newStatus);

            Console.WriteLine(
                $"Ride status updated to {newStatus}.");
        }

        // Reservation operations
        public bool CreateReservation(
            string visitorId,
            string rideId,
            string timeSlot)
        {
            Visitor? visitor = FindVisitor(visitorId);

            if (visitor == null)
            {
                Console.WriteLine(
                    "RESERVATION FAILED");
                Console.WriteLine(
                    "Reason: Visitor does not exist.");
                return false;
            }

            Ride? ride = FindRide(rideId);

            if (ride == null)
            {
                Console.WriteLine(
                    "RESERVATION FAILED");
                Console.WriteLine(
                    "Reason: Ride does not exist.");
                return false;
            }

            if (ride.Status != RideStatus.Open)
            {
                Console.WriteLine(
                    "RESERVATION FAILED");
                Console.WriteLine(
                    $"Reason: Ride is {ride.Status}.");
                return false;
            }

            Ticket? ticket = FindActiveTicket(visitorId);

            if (ticket == null)
            {
                Console.WriteLine(
                    "RESERVATION FAILED");
                Console.WriteLine(
                    "Reason: Visitor does not have a valid ticket.");
                return false;
            }

            bool duplicate = reservations.Any(r =>
                r.VisitorId == visitorId &&
                r.RideId == rideId &&
                r.TimeSlot == timeSlot &&
                r.Status == ReservationStatus.Active);

            if (duplicate)
            {
                Console.WriteLine(
                    "RESERVATION FAILED");
                Console.WriteLine(
                    "Reason: Visitor already has a reservation for this slot.");
                return false;
            }

            int slotReservations = reservations.Count(r =>
                r.RideId == rideId &&
                r.TimeSlot == timeSlot &&
                r.Status == ReservationStatus.Active);

            if (slotReservations >= ride.MaxCapacity)
            {
                Console.WriteLine(
                    "RESERVATION FAILED");
                Console.WriteLine(
                    "Reason: Ride has reached maximum capacity for the selected time slot.");
                return false;
            }

            RideReservation reservation =
                new RideReservation(
                    Guid.NewGuid().ToString(),
                    visitorId,
                    rideId,
                    timeSlot);

            reservations.Add(reservation);

            Console.WriteLine(
                "RESERVATION CREATED");

            return true;
        }

        public bool CancelReservation(
            string visitorId,
            string rideId,
            string timeSlot)
        {
            RideReservation? reservation =
                reservations.FirstOrDefault(r =>
                    r.VisitorId == visitorId &&
                    r.RideId == rideId &&
                    r.TimeSlot == timeSlot &&
                    r.Status == ReservationStatus.Active);

            if (reservation == null)
            {
                Console.WriteLine(
                    "Reservation not found.");
                return false;
            }

            reservation.Cancel();

            Console.WriteLine(
                "Reservation cancelled successfully.");

            return true;
        }

        // Staff operations
        public bool AssignEmployee(
            string employeeId,
            string rideId,
            string timePeriod)
        {
            Employee? employee =
                employees.FirstOrDefault(e =>
                    e.EmployeeId == employeeId);

            if (employee == null)
            {
                Console.WriteLine(
                    "Employee does not exist.");
                return false;
            }

            Ride? ride = FindRide(rideId);

            if (ride == null)
            {
                Console.WriteLine(
                    "Ride does not exist.");
                return false;
            }

            bool conflict = assignments.Any(a =>
                a.EmployeeId == employeeId &&
                a.TimePeriod == timePeriod);

            if (conflict)
            {
                Console.WriteLine(
                    "Assignment failed: Employee is already assigned during this period.");
                return false;
            }

            StaffAssignment assignment =
                new StaffAssignment(
                    Guid.NewGuid().ToString(),
                    employeeId,
                    rideId,
                    timePeriod);

            assignments.Add(assignment);

            employee.SetAvailability(false);

            Console.WriteLine(
                "Employee assigned successfully.");

            return true;
        }

        public void AddEmployee(Employee employee)
        {
            if (employees.Any(e =>
                e.EmployeeId == employee.EmployeeId))
            {
                Console.WriteLine(
                    "Employee ID already exists.");
                return;
            }

            employees.Add(employee);

            Console.WriteLine(
                "Employee added successfully.");
        }
    }
}