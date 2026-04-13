namespace Movie_StructureCode.Domain.Entities
{
    public class RefreshToken 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; }
        public string JwtId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastUsedDate { get; set; }
        public DateTime ExpriseDate { get; set; }

        public bool IsRevoked { get; set; } = false;
        public bool IsUsed { get; set; } = false;
        // forein key
        public Guid UserId { get; set; }
        public AppUser? AppUser { get; set; }

    }
}
