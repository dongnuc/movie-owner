namespace Movie_StructureCode.Domain.Entities
{
    public enum ShowingSeatStatus
    {
        Available,
        Locked,
        Booked
    }

    public class ShowingSeat : BaseEntities
    {
        public ShowingSeatStatus Status { get; set; } = ShowingSeatStatus.Available;

        public decimal Price { get; set; }

        /// <summary>
        /// Thời điểm hết hạn lock (dùng khi Status = Locked).
        /// Null nếu ghế đang Available hoặc đã Booked.
        /// </summary>
        public DateTime? LockedUntil { get; set; }

        // ── Foreign keys ──────────────────────────────────────────────────────

        public Guid ShowingId { get; set; }
        public Showing? Showing { get; set; }

        public Guid SeatId { get; set; }
        public Seat? Seat { get; set; }

        /// <summary>
        /// Null khi ghế chưa được đặt (Available / Locked).
        /// Có giá trị khi Status = Booked.
        /// </summary>
        public Guid? BookingId { get; set; }
        public Booking? Booking { get; set; }

        public Ticket? Ticket { get; set; }
    }
}
