namespace Movie_StructureCode.Domain.Entities
{
    public class Showing : BaseEntities
    {
        public DateTime TimeStart { get; set; }
        public decimal Price { get; set; }
        public int MaxTicketsPerUser { get; set; }


        //foreign key

        public Guid MovieId { get; set; }
        public Movie? Movie { get; set; }
        public Guid RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
