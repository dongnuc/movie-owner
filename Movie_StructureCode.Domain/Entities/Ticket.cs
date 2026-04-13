namespace Movie_StructureCode.Domain.Entities
{
    public enum TicketStatus
    {
        Active,
        Refunded
    }
    public class Ticket : BaseEntities
    {
        public decimal Price { get; set; }
        public TicketStatus TicketStatus { get; set; } = TicketStatus.Active;

        //foreign key

        public Guid BookingId { get; set; }
        public Booking? Booking { get; set; }
        public Guid ShowingSeatId { get; set; }
        public ShowingSeat? ShowingSeat { get; set; }
    }
}
