using Library.DataModel.Database;
using Library.Service.Dto;
using Microsoft.EntityFrameworkCore;

namespace Library.Service.Persistence
{
    public class LibraryRepository : ILibraryRepository 
    {
        private readonly LibraryDbContext _context;

        public LibraryRepository(LibraryDbContext context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<BookDto>> GetMostBorrowedBooksAsync(int limit)
        {
            if (limit <= 0)
                throw new ArgumentOutOfRangeException(nameof(limit));
            return await _context.Books
             .OrderByDescending(b => b.BorrowRecords!.Count)
             .Take(limit)
             .Select(b => new BookDto
             {
                 Id = b.Id,
                 Title = b.Title,
                 Author = b.Author,
                 TotalPages = b.TotalPages,
                 TotalCopies = b.TotalCopies,
                 AvailableCopies = b.TotalCopies - b.BorrowRecords!.Count(br => br.ReturnedDate == null),
                 BorrowedCopies = b.BorrowRecords!.Count(),
                 ISBN = b.ISBN
             })
             .ToListAsync();
        }
        public async Task<int> GetBorrowedCopiesCountAsync(int bookId)
        {
            return await _context.BorrowRecords
                .CountAsync(br => br.BookId == bookId && br.ReturnedDate == null);
        }
        public async Task<int> GetAvailableCopiesCountAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return 0; 
            }
            int borrowedCopiesCount = await GetBorrowedCopiesCountAsync(bookId);
            return book.TotalCopies - borrowedCopiesCount;
        }
        public async Task<IEnumerable<UserDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int limit)
        {
            return await _context.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    BorrowedBooksCount = u.BorrowRecords!
                        .Where(br => br.BorrowedDate >= startDate && br.BorrowedDate <= endDate)
                        .Count()
                })
                .OrderByDescending(u => u.BorrowedBooksCount)
                .Take(limit)
                .ToListAsync();
        }
        public async Task<IEnumerable<BookDto>> GetBooksBorrowedByUserAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.BorrowRecords
                .Where(br => br.UserId == userId && br.BorrowedDate >= startDate && br.BorrowedDate <= endDate)
                .Include(br => br.Book)
                .Select(br => new BookDto
                {
                    Id = br.Book!.Id,
                    Title = br.Book.Title,
                    Author = br.Book.Author,
                    TotalPages = br.Book.TotalPages,
                    TotalCopies = br.Book.TotalCopies,
                    ISBN = br.Book.ISBN,
                    AvailableCopies = br.Book.TotalCopies - br.Book.BorrowRecords!.Count(br => br.ReturnedDate == null),
                    BorrowedCopies= br.Book.BorrowRecords!.Count()
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<BookDto>> GetRelatedBooksAsync(int bookId)
        {
            var usersWhoBorrowed = await _context.BorrowRecords
                .Where(br => br.BookId == bookId)
                .Select(br => br.UserId)
                .Distinct()
                .ToListAsync();

            return await _context.BorrowRecords
                .Where(br => usersWhoBorrowed.Contains(br.UserId) && br.BookId != bookId)
                .Include(br => br.Book)
                .Select(br => new BookDto
                {
                    Id = br.Book!.Id,
                    Title = br.Book.Title,
                    Author = br.Book.Author,
                    TotalPages = br.Book.TotalPages,
                    ISBN = br.Book.ISBN,
                    AvailableCopies = br.Book.TotalCopies - br.Book.BorrowRecords!.Count(br => br.ReturnedDate == null),
                    BorrowedCopies = br.Book.BorrowRecords!.Count(),    
                })
                .Distinct()
                .ToListAsync();
        }
        public async Task<float> GetReadRateAsync(int bookId)
        {
            if (bookId <= 0)
                throw new ArgumentOutOfRangeException(nameof(bookId));
            var borrowRecords = await _context.BorrowRecords
               .Where(br => br.BookId == bookId && br.ReturnedDate != null)
               .Include(br => br.Book)  
               .ToListAsync();

            if (borrowRecords.Count == 0)
            {
                return 0f;
            }

            var totalDays = borrowRecords.Sum(br => (br.ReturnedDate!.Value - br.BorrowedDate).TotalDays);

            if (totalDays == 0)
            {
                return 0f; 
            }

            var totalPages = borrowRecords.First().Book?.TotalPages
                ?? throw new InvalidOperationException("The book associated with the borrow records is null.");

            // Calculate the read rate (pages per day)
            return totalPages / (float)totalDays;
        }

        public async Task<BookDetailsDto> GetBookDetailsAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with Id {bookId} not found.");
            }
            var bookDetailsDto = new BookDetailsDto
            {
                Title = book.Title,
                TotalCopies = book.TotalCopies
            };
            return bookDetailsDto;
        }
    }
}
