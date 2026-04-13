namespace Movie_StructureCode.Domain.Entities
{
    public class Category : BaseEntities
    {
        public string Name { get; set; }

        public ICollection<Movie>? Movies { get; set; }
    }
}
