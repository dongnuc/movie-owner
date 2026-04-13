namespace Movie_StructureCode.Domain.Entities
{
    public class SeatType : BaseEntities
    {
        public string Name { get; set; }
        public decimal Surcharge { get; set; }

        public ICollection<Seat>? Seats { get; set; }
    }
}
