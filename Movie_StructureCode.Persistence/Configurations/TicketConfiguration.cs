using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie_StructureCode.Domain.Entities;

namespace Movie_StructureCode.Persistence.Configurations
{
    public sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable(nameof(Ticket));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(x => x.TicketStatus)
                .IsRequired()
                .HasDefaultValue(TicketStatus.Active);

            builder.Property(x => x.DateCreate)
                .IsRequired();

            builder.Property(x => x.DateUpdate)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasOne(x => x.Booking)
                .WithMany(x => x.Tickets)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
