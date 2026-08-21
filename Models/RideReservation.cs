using Horizon_Adventure_Park.Enums;


namespace Horizon_Adventure_Park.Models
{
    public class RideReservation
    {
        public string ReservationId { get; set; }
        public string VisitorId { get; set; }
        public string RideId { get; set; }
        public string TimeSlot { get; set; }
        public ReservationStatus Status { get; set; }

        public RideReservation(
            string reservationId,
            string visitorId,
            string rideId,
            string timeSlot)
        {
            ReservationId = reservationId;
            VisitorId = visitorId;
            RideId = rideId;
            TimeSlot = timeSlot;
            Status = ReservationStatus.Active;
        }

        public void Cancel()
        {
            Status = ReservationStatus.Cancelled;
        }
    }
}