using Horizon_Adventure_Park.Enums;



namespace Horizon_Adventure_Park.Models
{
    public class Ride
    {
        public string RideId { get; set; }
        public string Name { get; set; }
        public RideType Type { get; set; }

        public int MinAge { get; set; }
        public double MinHeight { get; set; }
        public bool RequiresAdult { get; set; }

        public int MaxCapacity { get; set; }
        public int CurrentOccupancy { get; set; }

        public RideStatus Status { get; set; }

        public Ride(
            string rideId,
            string name,
            RideType type,
            int minAge,
            double minHeight,
            bool requiresAdult,
            int maxCapacity)
        {
            RideId = rideId;
            Name = name;
            Type = type;
            MinAge = minAge;
            MinHeight = minHeight;
            RequiresAdult = requiresAdult;
            MaxCapacity = maxCapacity;
            CurrentOccupancy = 0;
            Status = RideStatus.Open;
        }

        public bool HasCapacity()
        {
            return CurrentOccupancy < MaxCapacity;
        }

        public string CheckEligibility(Visitor visitor, Ticket ticket)
        {
            if (Status == RideStatus.Closed)
                return "Ride is currently closed.";

            if (Status == RideStatus.UnderMaintenance)
                return "Ride is currently under maintenance.";

            if (ticket == null)
                return "Visitor does not have a ticket.";

            if (!ticket.IsValid())
                return "Visitor's ticket is invalid, expired, or cancelled.";

            if (ticket.Type != TicketType.VIP &&
                visitor.Category != VisitorType.VIP)
            {
                //access will be checked by the system.
            }

            if (visitor.Age < MinAge)
                return $"Visitor does not meet the minimum age requirement ({MinAge}).";

            if (visitor.Height < MinHeight)
                return $"Visitor does not meet the minimum height requirement ({MinHeight}cm).";

            if (RequiresAdult && !visitor.HasAccompanyingAdult)
                return "Visitor must be accompanied by an adult.";

            if (!HasCapacity())
                return "Ride has reached maximum capacity.";

            return "Eligible";
        }

        public bool AdmitVisitor()
        {
            if (!HasCapacity())
                return false;

            if (Status != RideStatus.Open)
                return false;

            CurrentOccupancy++;
            return true;
        }

        public void UpdateStatus(RideStatus newStatus)
        {
            Status = newStatus;
        }
    }
}