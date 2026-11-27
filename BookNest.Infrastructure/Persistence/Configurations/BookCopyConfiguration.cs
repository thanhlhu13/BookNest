
using BookNest.Domain.Entities;
using BookNest.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookNest.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the BookCopy entity.
/// </summary>
public class BookCopyConfiguration : IEntityTypeConfiguration<BookCopy>
{
    public void Configure(EntityTypeBuilder<BookCopy> builder)
    {
        builder.ToTable("BookCopies");

        builder.HasKey(bc => bc.Id);

        builder.Property(bc => bc.CopyCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(bc => bc.CopyCode)
            .IsUnique();

        builder.Property(bc => bc.Status)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(BookCopyStatus.Available);

        builder.Property(bc => bc.CreatedAt)
            .IsRequired();

        builder.Property(bc => bc.UpdatedAt);

        builder.HasOne(bc => bc.Book)
            .WithMany()
            .HasForeignKey(bc => bc.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(bc => bc.BookId);
    }
}
