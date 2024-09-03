using Library.DataModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataModel.Entities
{
    public class BorrowRecordEntityConfiguration : IEntityTypeConfiguration<BorrowRecord>
    {
        public void Configure(EntityTypeBuilder<BorrowRecord> builder)
        {
            builder.ToTable("BorrowRecords");

            builder.HasKey(br => br.Id);

            builder.Property(br => br.BookId)
                .IsRequired();

            builder.Property(br => br.UserId)
                .IsRequired();

            builder.Property(br => br.BorrowedDate)
                .IsRequired();

            builder.Property(br => br.ReturnedDate)
                .IsRequired(false);

            builder.HasOne(br => br.Book)
                .WithMany(b => b.BorrowRecords)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(br => br.User)
                .WithMany(u => u.BorrowRecords)
                .HasForeignKey(br => br.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
