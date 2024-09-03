using Library.DataModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataModel.Entities
{
    public class BookEntityConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            // Table name
            builder.ToTable("Books");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .HasMaxLength(255);

            builder.Property(b => b.Author)
                .HasMaxLength(255);

            builder.Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.TotalPages)
                .IsRequired();

            builder.Property(b => b.TotalCopies)
                .IsRequired();

            builder.HasMany(b => b.BorrowRecords)
                .WithOne(br => br.Book)
                .HasForeignKey(br => br.BookId);

            builder.HasIndex(b => b.ISBN)
                .IsUnique()
                .HasDatabaseName("IX_Books_ISBN");
        }
    }
}
