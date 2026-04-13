namespace Movie_StructureCode.Domain.Entities
{
    public enum StatusBooking
    {
        Pending,
        Confirmed,
        Cancelled
    }

    public class Booking : BaseEntities
    {
        public decimal TotalAmount { get; set; }
        public StatusBooking StatusBooking { get; set; } = StatusBooking.Pending;

        //foreign key

        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public Guid ShowingId { get; set; }
        public Showing? Showing { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }
    }
}
