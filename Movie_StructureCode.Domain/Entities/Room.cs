namespace Movie_StructureCode.Domain.Entities
{
    public class Room : BaseEntities
    {
        public string Name { get; set; }
        public int TotalSeat { get; set; }
        public int TotalRow { get; set; }
        public int TotalCol { get; set; }

        // foreign key
        public Guid TheaterId { get; set; }
        public Theater? Theater { get; set; }

        public ICollection<Seat>? Seats { get; set; }
        public ICollection<Showing>? Showings { get; set; }
    }
}
