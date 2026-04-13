namespace Movie_StructureCode.Domain.Entities
{
    public class Theater : BaseEntities
    {
        public string Name { get; set; }
        public string? Location { get; set; }

        public ICollection<Room>? Rooms { get; set; }
    }
}
