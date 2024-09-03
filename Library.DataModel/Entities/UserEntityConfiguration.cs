using Library.DataModel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.DataModel.Entities
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .HasMaxLength(100);  

            builder.Property(u => u.Email)
                .HasMaxLength(255);  

            builder.HasMany(u => u.BorrowRecords)
                .WithOne(br => br.User)
                .HasForeignKey(br => br.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
