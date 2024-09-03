using Library.DataModel.Database;
using Library.DataModel.Models;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;

namespace LibraryService.Test
{
    public class SqlTestFixture : IDisposable
    {
        public LibraryDbContext DbContext { get; set; }

        private readonly MsSqlContainer _mssqlContainer;

        public SqlTestFixture()
        {
            _mssqlContainer = new MsSqlBuilder()
                .WithPassword("Adenike62")
                .Build();

            _mssqlContainer.StartAsync().Wait();

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseSqlServer(_mssqlContainer.GetConnectionString())
                .Options;

            DbContext = new LibraryDbContext(options);

            SeedData();
        }

        private void SeedData()
        {
            DbContext.Database.EnsureCreated();

            // Seed data
            var book1 = new Book
            {
                Title = "Test Book1",
                Author = "TestAuthor 1",
                TotalPages = 500,
                ISBN = "123-456-9783-365",
                TotalCopies = 5,
            };
            var book2 = new Book
            {
                Title = "Test Book12",
                Author = "TestAuthor 2",
                TotalPages = 190,
                ISBN = "123-456-9783-365",
                TotalCopies = 19,
            };
            var user1 = new User { Name = "Tolu Caleb", Email ="tolu@test.com" };
            var user2 = new User { Name = "Bola idowu", Email = "bola@test.com" };

            DbContext.Books.AddRange(book1, book2);
            DbContext.Users.AddRange(user1, user2);

            DbContext.BorrowRecords.AddRange(
                new BorrowRecord
                {
                    BookId = book1.Id,
                    UserId = user1.Id,
                    BorrowedDate = DateTime.Now.AddDays(-10),
                    ReturnedDate = DateTime.Now.AddDays(5),
                    Book = book1,
                    User = user1
                },
                new BorrowRecord
                {
                    BookId = book2.Id,
                    UserId = user2.Id,
                    BorrowedDate = DateTime.Now.AddDays(-20),
                    ReturnedDate = DateTime.Now.AddDays(5),
                    Book = book2,
                    User = user2
                }
            );

            DbContext.SaveChangesAsync().Wait();
        }

        public void Dispose()
        {
            _mssqlContainer.StopAsync().Wait();
        }
    }
}
 