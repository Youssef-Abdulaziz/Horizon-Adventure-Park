using Horizon_Adventure_Park.Enums;


namespace Horizon_Adventure_Park.Models
{
    public class Ticket
    {
        public string TicketId { get; set; }
        public string VisitorId { get; set; }
        public TicketType Type { get; set; }
        public decimal Price { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public TicketStatus Status { get; set; }

        public Ticket(
            string ticketId,
            string visitorId,
            TicketType type,
            decimal price,
            DateTime issueDate,
            DateTime expiryDate)
        {
            TicketId = ticketId;
            VisitorId = visitorId;
            Type = type;
            Price = price;
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            Status = TicketStatus.Active;
        }

        public bool IsValid()
        {
            if (Status != TicketStatus.Active)
                return false;

            if (DateTime.Now > ExpiryDate)
            {
                Status = TicketStatus.Expired;
                return false;
            }

            return true;
        }

        public void Deactivate()
        {
            Status = TicketStatus.Cancelled;
        }
    }
}