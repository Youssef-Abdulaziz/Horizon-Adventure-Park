using Horizon_Adventure_Park.Enums;
using Horizon_Adventure_Park.Models;


namespace Horizon_Adventure_Park.Services
{
    public class ParkSystem
    {
        // Arrays
        private Visitor[] visitors = new Visitor[100];
        private Ticket[] tickets = new Ticket[100];
        private Ride[] rides = new Ride[20];
        private RideReservation[] reservations = new RideReservation[200];
        private Employee[] employees = new Employee[50];
        private StaffAssignment[] assignments = new StaffAssignment[100];

        private int visitorCount = 0;
        private int ticketCount = 0;
        private int rideCount = 0;
        private int reservationCount = 0;
        private int employeeCount = 0;
        private int assignmentCount = 0;

        // Used to generate sequential ticket IDs (T001, T002, ...)
        private int ticketSequence = 0;


        // VISITOR OPERATIONS

        public bool RegisterVisitor(Visitor visitor)
        {
            if (visitorCount >= visitors.Length)
            {
                Console.WriteLine("Visitor storage is full.");
                return false;
            }

            // Prevent duplicate visitor IDs
            for (int i = 0; i < visitorCount; i++)
            {
                if (visitors[i].VisitorId == visitor.VisitorId)
                {
                    Console.WriteLine(
                        "Registration failed: Visitor ID already exists.");

                    return false;
                }
            }

            visitors[visitorCount] = visitor;
            visitorCount++;

            Console.WriteLine("Visitor registered successfully.");

            return true;
        }


        public Visitor? FindVisitor(string visitorId)
        {
            for (int i = 0; i < visitorCount; i++)
            {
                if (visitors[i].VisitorId == visitorId)
                {
                    return visitors[i];
                }
            }

            return null;
        }


        // TICKET OPERATIONS

        public bool IssueTicket(Ticket ticket)
        {
            if (ticketCount >= tickets.Length)
            {
                Console.WriteLine("Ticket storage is full.");
                return false;
            }

            Visitor? visitor = FindVisitor(ticket.VisitorId);

            if (visitor == null)
            {
                Console.WriteLine(
                    "Ticket issuance failed: Visitor does not exist.");

                return false;
            }

            // Check if visitor already has an active ticket
            for (int i = 0; i < ticketCount; i++)
            {
                if (tickets[i].VisitorId == ticket.VisitorId &&
                    tickets[i].IsValid())
                {
                    Console.WriteLine(
                        "Visitor already has an active ticket.");

                    return false;
                }
            }

            // Generate sequential Ticket ID: T001, T002, T003...
            ticketSequence++;
            ticket.TicketId = "T" + ticketSequence.ToString("D3");

            tickets[ticketCount] = ticket;
            ticketCount++;

            Console.WriteLine("Ticket issued successfully.");
            Console.WriteLine($"Ticket ID: {ticket.TicketId}");

            return true;
        }


        public Ticket? FindActiveTicket(string visitorId)
        {
            for (int i = 0; i < ticketCount; i++)
            {
                if (tickets[i].VisitorId == visitorId &&
                    tickets[i].IsValid())
                {
                    return tickets[i];
                }
            }

            return null;
        }


        public Ticket? FindTicketById(string ticketId)
        {
            for (int i = 0; i < ticketCount; i++)
            {
                if (tickets[i].TicketId == ticketId)
                {
                    return tickets[i];
                }
            }

            return null;
        }


        public bool DeactivateTicket(string ticketId)
        {
            Ticket? ticket = FindTicketById(ticketId);

            if (ticket == null)
            {
                Console.WriteLine("Ticket not found.");
                return false;
            }

            if (ticket.Status == TicketStatus.Cancelled)
            {
                Console.WriteLine("Ticket is already deactivated.");
                return false;
            }

            ticket.Deactivate();

            Console.WriteLine("Ticket deactivated successfully.");

            return true;
        }


        public bool UpdateTicketStatus(
            string ticketId,
            TicketStatus newStatus)
        {
            Ticket? ticket = FindTicketById(ticketId);

            if (ticket == null)
            {
                Console.WriteLine("Ticket not found.");
                return false;
            }

            ticket.Status = newStatus;

            Console.WriteLine(
                $"Ticket status updated to {newStatus}.");

            return true;
        }


        // RIDE OPERATIONS

        public void AddRide(Ride ride)
        {
            if (rideCount >= rides.Length)
            {
                Console.WriteLine("Ride storage is full.");
                return;
            }

            // Prevent duplicate ride IDs
            for (int i = 0; i < rideCount; i++)
            {
                if (rides[i].RideId == ride.RideId)
                {
                    Console.WriteLine(
                        "Ride ID already exists.");

                    return;
                }
            }

            rides[rideCount] = ride;
            rideCount++;

            Console.WriteLine("Ride added successfully.");
        }


        public Ride? FindRide(string rideId)
        {
            for (int i = 0; i < rideCount; i++)
            {
                if (rides[i].RideId == rideId)
                {
                    return rides[i];
                }
            }

            return null;
        }


        public void ViewRideOccupancy()
        {
            if (rideCount == 0)
            {
                Console.WriteLine("No rides available.");
                return;
            }

            Console.WriteLine(
                $"{"Ride ID",-8}{"Name",-25}{"Status",-18}{"Occupancy",-12}{"Capacity"}");

            Console.WriteLine(new string('-', 75));

            for (int i = 0; i < rideCount; i++)
            {
                Ride ride = rides[i];

                Console.WriteLine(
                    $"{ride.RideId,-8}{ride.Name,-25}{ride.Status,-18}{ride.CurrentOccupancy,-12}{ride.MaxCapacity}");
            }
        }


        public bool ValidateRideAccess(
            string visitorId,
            string rideId)
        {
            Visitor? visitor = FindVisitor(visitorId);

            if (visitor == null)
            {
                Console.WriteLine("ACCESS DENIED");
                Console.WriteLine(
                    "Reason: Visitor does not exist.");

                return false;
            }

            Ride? ride = FindRide(rideId);

            if (ride == null)
            {
                Console.WriteLine("ACCESS DENIED");
                Console.WriteLine(
                    "Reason: Ride does not exist.");

                return false;
            }

            Ticket? ticket = FindActiveTicket(visitorId);

            if (ticket == null)
            {
                Console.WriteLine("ACCESS DENIED");
                Console.WriteLine(
                    "Reason: Visitor does not have a valid ticket.");

                return false;
            }

            string result =
                ride.CheckEligibility(visitor, ticket);

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
                Console.WriteLine(
                    "Ride does not exist.");

                return;
            }

            ride.UpdateStatus(newStatus);

            Console.WriteLine(
                $"Ride status updated to {newStatus}.");
        }


        // RESERVATION OPERATIONS

        public bool CreateReservation(
            string visitorId,
            string rideId,
            string timeSlot)
        {
            if (reservationCount >= reservations.Length)
            {
                Console.WriteLine(
                    "Reservation storage is full.");

                return false;
            }

            Visitor? visitor =
                FindVisitor(visitorId);

            if (visitor == null)
            {
                Console.WriteLine(
                    "RESERVATION FAILED");

                Console.WriteLine(
                    "Reason: Visitor does not exist.");

                return false;
            }

            Ride? ride =
                FindRide(rideId);

            if (ride == null)
            {
                Console.WriteLine(
                    "RESERVATION FAILED");

                Console.WriteLine(
                    "Reason: Ride does not exist.");

                return false;
            }

            // Ride must be open
            if (ride.Status != RideStatus.Open)
            {
                Console.WriteLine(
                    "RESERVATION FAILED");

                Console.WriteLine(
                    $"Reason: Ride is {ride.Status}.");

                return false;
            }

            // Visitor must have a valid ticket
            Ticket? ticket =
                FindActiveTicket(visitorId);

            if (ticket == null)
            {
                Console.WriteLine(
                    "RESERVATION FAILED");

                Console.WriteLine(
                    "Reason: Visitor does not have a valid ticket.");

                return false;
            }

            // Check duplicate reservation
            for (int i = 0; i < reservationCount; i++)
            {
                if (reservations[i].VisitorId == visitorId &&
                    reservations[i].RideId == rideId &&
                    reservations[i].TimeSlot == timeSlot &&
                    reservations[i].Status ==
                    ReservationStatus.Active)
                {
                    Console.WriteLine(
                        "RESERVATION FAILED");

                    Console.WriteLine(
                        "Reason: Visitor already has a reservation for this slot.");

                    return false;
                }
            }

            // Count reservations for this ride and time slot
            int slotReservations = 0;

            for (int i = 0; i < reservationCount; i++)
            {
                if (reservations[i].RideId == rideId &&
                    reservations[i].TimeSlot == timeSlot &&
                    reservations[i].Status ==
                    ReservationStatus.Active)
                {
                    slotReservations++;
                }
            }

            // Check capacity
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

            reservations[reservationCount] = reservation;
            reservationCount++;

            Console.WriteLine(
                "RESERVATION CREATED");

            return true;
        }


        public bool CancelReservation(
            string visitorId,
            string rideId,
            string timeSlot)
        {
            for (int i = 0; i < reservationCount; i++)
            {
                if (reservations[i].VisitorId == visitorId &&
                    reservations[i].RideId == rideId &&
                    reservations[i].TimeSlot == timeSlot &&
                    reservations[i].Status ==
                    ReservationStatus.Active)
                {
                    reservations[i].Cancel();

                    Console.WriteLine(
                        "Reservation cancelled successfully.");

                    return true;
                }
            }

            Console.WriteLine(
                "Reservation not found.");

            return false;
        }


        // EMPLOYEE OPERATIONS

        public void AddEmployee(Employee employee)
        {
            if (employeeCount >= employees.Length)
            {
                Console.WriteLine(
                    "Employee storage is full.");

                return;
            }

            //Prevent duplicate employee IDs
            for (int i = 0; i < employeeCount; i++)
            {
                if (employees[i].EmployeeId ==
                    employee.EmployeeId)
                {
                    Console.WriteLine(
                        "Employee ID already exists.");

                    return;
                }
            }

            employees[employeeCount] = employee;
            employeeCount++;

            Console.WriteLine(
                "Employee added successfully.");
        }


        public bool AssignEmployee(
            string employeeId,
            string rideId,
            string timePeriod)
        {
            if (assignmentCount >= assignments.Length)
            {
                Console.WriteLine(
                    "Assignment storage is full.");

                return false;
            }

            Employee? employee = null;

            // Find employee using array
            for (int i = 0; i < employeeCount; i++)
            {
                if (employees[i].EmployeeId ==
                    employeeId)
                {
                    employee = employees[i];
                    break;
                }
            }

            if (employee == null)
            {
                Console.WriteLine(
                    "Employee does not exist.");

                return false;
            }

            // Check ride
            Ride? ride =
                FindRide(rideId);

            if (ride == null)
            {
                Console.WriteLine(
                    "Ride does not exist.");

                return false;
            }

            // Check for conflicting assignment
            for (int i = 0; i < assignmentCount; i++)
            {
                if (assignments[i].EmployeeId ==
                    employeeId &&
                    assignments[i].TimePeriod ==
                    timePeriod)
                {
                    Console.WriteLine(
                        "Assignment failed: Employee is already assigned during this period.");

                    return false;
                }
            }

            StaffAssignment assignment =
                new StaffAssignment(
                    Guid.NewGuid().ToString(),
                    employeeId,
                    rideId,
                    timePeriod);

            assignments[assignmentCount] = assignment;
            assignmentCount++;

            employee.SetAvailability(false);

            Console.WriteLine(
                "Employee assigned successfully.");

            return true;
        }
    }
}