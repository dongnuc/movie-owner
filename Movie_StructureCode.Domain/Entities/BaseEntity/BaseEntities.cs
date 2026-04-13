namespace Movie_StructureCode.Domain.Entities
{
    public class BaseEntities 
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = false;
    }
}
