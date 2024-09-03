using Library.Service.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Service.Services
{
    public interface ILibraryService
    {
        Task<IEnumerable<BookDto>> GetMostBorrowedBooksAsync(int limit, CancellationToken cancellationToken);
        Task<BookStatusDto> GetBookStatusAsync(int bookId, CancellationToken cancellationToken);   
        Task<IEnumerable<UserDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int limit, CancellationToken cancellationToken);
        Task<IEnumerable<BookDto>> GetBooksBorrowedByUserAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        Task<IEnumerable<BookDto>> GetRelatedBooksAsync(int bookId, CancellationToken cancellationToken);
        Task<float> GetReadRateAsync(int bookId, CancellationToken cancellationToken);

    }
}
