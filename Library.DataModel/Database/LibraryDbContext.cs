using Library.DataModel.Entities;
using Library.DataModel.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.DataModel.Database
{
    public class LibraryDbContext : DbContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<BorrowRecord> BorrowRecords { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookEntityConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new BorrowRecordEntityConfiguration());
        }
    }
}
