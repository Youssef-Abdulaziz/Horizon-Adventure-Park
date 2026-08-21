using Horizon_Adventure_Park.Enums;
using Horizon_Adventure_Park.Models;

using Horizon_Adventure_Park.Services;


namespace Horizon_Adventure_Park
{
    internal class Program
    {
        static ThemeParkSystem system = new ThemeParkSystem();

        static void Main(string[] args)
        {
            SeedData();

            bool running = true;

            while (running)
            {
                DisplayMenu();

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterVisitor();
                        break;

                    case "2":
                        IssueTicket();
                        break;

                    case "3":
                        ValidateRideAccess();
                        break;

                    case "4":
                        CreateReservation();
                        break;

                    case "5":
                        ManageRideStatus();
                        break;

                    case "6":
                        AssignStaff();
                        break;

                    case "7":
                        running = false;
                        Console.WriteLine();
                        Console.WriteLine("Thank you for using Horizon Adventure Park.");
                        Console.WriteLine("System closed.");
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Invalid option. Please choose 1-7.");
                        break;
                }

                if (running)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press ENTER to continue...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        // ============================================
        // MAIN MENU
        // ============================================

        static void DisplayMenu()
        {
            Console.WriteLine("==============================================");
            Console.WriteLine("       HORIZON ADVENTURE PARK");
            Console.WriteLine("          OPERATIONS SYSTEM");
            Console.WriteLine("==============================================");
            Console.WriteLine("1. Register Visitor");
            Console.WriteLine("2. Issue Ticket");
            Console.WriteLine("3. Validate Ride Access");
            Console.WriteLine("4. Create Reservation");
            Console.WriteLine("5. Manage Ride Status");
            Console.WriteLine("6. Assign Staff");
            Console.WriteLine("7. Exit");
            Console.WriteLine("==============================================");
            Console.Write("Select an option: ");
        }

        // ============================================
        // REGISTER VISITOR
        // ============================================

        static void RegisterVisitor()
        {
            Console.WriteLine();
            Console.WriteLine("========== REGISTER VISITOR ==========");

            Console.Write("Enter Visitor ID: ");
            string visitorId = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(visitorId))
            {
                Console.WriteLine("Visitor ID cannot be empty.");
                return;
            }

            Console.Write("Enter Name: ");
            string name = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty.");
                return;
            }

            int age = ReadInt("Enter Age: ");

            if (age < 0)
            {
                Console.WriteLine("Age cannot be negative.");
                return;
            }

            double height = ReadDouble("Enter Height (cm): ");

            if (height <= 0)
            {
                Console.WriteLine("Height must be greater than 0.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Visitor Categories:");
            Console.WriteLine("1. General");
            Console.WriteLine("2. VIP");
            Console.WriteLine("3. Child");
            Console.WriteLine("4. Senior");
            Console.WriteLine("5. Staff Accompanied Minor");

            int categoryChoice = ReadInt("Select category: ");

            VisitorType category;

            switch (categoryChoice)
            {
                case 1:
                    category = VisitorType.General;
                    break;

                case 2:
                    category = VisitorType.VIP;
                    break;

                case 3:
                    category = VisitorType.Child;
                    break;

                case 4:
                    category = VisitorType.Senior;
                    break;

                case 5:
                    category = VisitorType.StaffAccompaniedMinor;
                    break;

                default:
                    Console.WriteLine("Invalid visitor category.");
                    return;
            }

            bool hasAdult = false;

            if (category == VisitorType.Child ||
                category == VisitorType.StaffAccompaniedMinor)
            {
                Console.Write("Has accompanying adult? (y/n): ");

                string answer =
                    Console.ReadLine()?.Trim().ToLower() ?? "n";

                hasAdult = answer == "y";
            }

            Visitor visitor = new Visitor(
                visitorId,
                name,
                age,
                height,
                category,
                hasAdult
            );

            system.RegisterVisitor(visitor);
        }

        // ============================================
        // ISSUE TICKET
        // ============================================

        static void IssueTicket()
        {
            Console.WriteLine();
            Console.WriteLine("========== ISSUE TICKET ==========");

            Console.Write("Enter Visitor ID: ");
            string visitorId = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(visitorId))
            {
                Console.WriteLine("Visitor ID cannot be empty.");
                return;
            }

            Visitor? visitor = system.FindVisitor(visitorId);

            if (visitor == null)
            {
                Console.WriteLine("Ticket issuance failed.");
                Console.WriteLine("Reason: Visitor does not exist.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Visitor: {visitor.VisitorName}");
            Console.WriteLine($"Category: {visitor.Category}");

            Console.WriteLine();
            Console.WriteLine("Ticket Types:");
            Console.WriteLine("1. Regular");
            Console.WriteLine("2. VIP");

            int choice = ReadInt("Select ticket type: ");

            TicketType type;
            decimal price;

            switch (choice)
            {
                case 1:
                    type = TicketType.Regular;
                    price = 50m;
                    break;

                case 2:
                    type = TicketType.VIP;
                    price = 100m;
                    break;

                default:
                    Console.WriteLine("Invalid ticket type.");
                    return;
            }

            // VIP visitors should receive VIP tickets.
            if (visitor.Category == VisitorType.VIP &&
                type != TicketType.VIP)
            {
                Console.WriteLine();
                Console.WriteLine("Ticket issuance failed.");
                Console.WriteLine(
                    "Reason: VIP visitors must receive a VIP ticket.");
                return;
            }

            DateTime issueDate = DateTime.Now;
            DateTime expiryDate = issueDate.AddDays(1);

            Ticket ticket = new Ticket(
                Guid.NewGuid().ToString(),
                visitorId,
                type,
                price,
                issueDate,
                expiryDate
            );

            system.IssueTicket(ticket);
        }

        // ============================================
        // VALIDATE RIDE ACCESS
        // ============================================

        static void ValidateRideAccess()
        {
            Console.WriteLine();
            Console.WriteLine("========== VALIDATE RIDE ACCESS ==========");

            Console.Write("Enter Visitor ID: ");
            string visitorId = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(visitorId))
            {
                Console.WriteLine("Visitor ID cannot be empty.");
                return;
            }

            Console.Write("Enter Ride ID: ");
            string rideId = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(rideId))
            {
                Console.WriteLine("Ride ID cannot be empty.");
                return;
            }

            Console.WriteLine();

            system.ValidateRideAccess(
                visitorId,
                rideId
            );
        }

        // ============================================
        // CREATE RESERVATION
        // ============================================

        static void CreateReservation()
        {
            Console.WriteLine();
            Console.WriteLine("========== CREATE RESERVATION ==========");

            Console.Write("Enter Visitor ID: ");
            string visitorId = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(visitorId))
            {
                Console.WriteLine("Visitor ID cannot be empty.");
                return;
            }

            Console.Write("Enter Ride ID: ");
            string rideId = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(rideId))
            {
                Console.WriteLine("Ride ID cannot be empty.");
                return;
            }

            Console.Write("Enter Desired Time Slot (e.g. 14:00): ");
            string timeSlot = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(timeSlot))
            {
                Console.WriteLine("Time slot cannot be empty.");
                return;
            }

            Console.WriteLine();

            system.CreateReservation(
                visitorId,
                rideId,
                timeSlot
            );
        }

        // ============================================
        // MANAGE RIDE STATUS
        // ============================================

        static void ManageRideStatus()
        {
            Console.WriteLine();
            Console.WriteLine("========== MANAGE RIDE STATUS ==========");

            Console.Write("Enter Ride ID: ");
            string rideId = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(rideId))
            {
                Console.WriteLine("Ride ID cannot be empty.");
                return;
            }

            Ride? ride = system.FindRide(rideId);

            if (ride == null)
            {
                Console.WriteLine("Ride does not exist.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"Ride: {ride.Name}");
            Console.WriteLine($"Current Status: {ride.Status}");

            Console.WriteLine();
            Console.WriteLine("Ride Status:");
            Console.WriteLine("1. Open");
            Console.WriteLine("2. Closed");
            Console.WriteLine("3. Under Maintenance");

            int choice = ReadInt("Select new status: ");

            RideStatus status;

            switch (choice)
            {
                case 1:
                    status = RideStatus.Open;
                    break;

                case 2:
                    status = RideStatus.Closed;
                    break;

                case 3:
                    status = RideStatus.UnderMaintenance;
                    break;

                default:
                    Console.WriteLine("Invalid ride status.");
                    return;
            }

            system.UpdateRideStatus(
                rideId,
                status
            );
        }

        // ============================================
        // ASSIGN STAFF
        // ============================================

        static void AssignStaff()
        {
            Console.WriteLine();
            Console.WriteLine("========== ASSIGN STAFF ==========");

            Console.Write("Enter Employee ID: ");
            string employeeId = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(employeeId))
            {
                Console.WriteLine("Employee ID cannot be empty.");
                return;
            }

            Console.Write("Enter Ride ID: ");
            string rideId = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(rideId))
            {
                Console.WriteLine("Ride ID cannot be empty.");
                return;
            }

            Console.Write("Enter Time Period (e.g. Morning): ");
            string timePeriod = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(timePeriod))
            {
                Console.WriteLine("Time period cannot be empty.");
                return;
            }

            Console.WriteLine();

            system.AssignEmployee(
                employeeId,
                rideId,
                timePeriod
            );
        }

        // ============================================
        // INTEGER INPUT VALIDATION
        // ============================================

        static int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);

                string? input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid input. Please enter a number."
                );
            }
        }

        // ============================================
        // DOUBLE INPUT VALIDATION
        // ============================================

        static double ReadDouble(string message)
        {
            while (true)
            {
                Console.Write(message);

                string? input = Console.ReadLine();

                if (double.TryParse(input, out double value))
                {
                    return value;
                }

                Console.WriteLine(
                    "Invalid input. Please enter a valid number."
                );
            }
        }

        // ============================================
        // SAMPLE DATA
        // ============================================

        static void SeedData()
        {
            // Rides
            system.AddRide(
                new Ride(
                    "R001",
                    "Thunder Peak Coaster",
                    RideType.Thrill,
                    12,
                    110,
                    false,
                    20
                )
            );

            system.AddRide(
                new Ride(
                    "R002",
                    "Splash Voyage",
                    RideType.Water,
                    8,
                    100,
                    true,
                    15
                )
            );

            system.AddRide(
                new Ride(
                    "R003",
                    "Family Carousel",
                    RideType.Family,
                    0,
                    80,
                    true,
                    30
                )
            );

            // Employees
            system.AddEmployee(
                new Employee(
                    "E001",
                    "John Smith",
                    EmployeeRole.RideOperator
                )
            );

            system.AddEmployee(
                new Employee(
                    "E002",
                    "Sarah Brown",
                    EmployeeRole.RideOperator
                )
            );

            system.AddEmployee(
                new Employee(
                    "E003",
                    "Mike Wilson",
                    EmployeeRole.TicketBoothStaff
                )
            );
        }
    }
}
