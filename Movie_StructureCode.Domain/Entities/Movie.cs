namespace Movie_StructureCode.Domain.Entities
{
    public class Movie : BaseEntities
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public decimal BasePrice { get; set; }
        public int Duration { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Showing>? Showings { get; set; }
    }
}
