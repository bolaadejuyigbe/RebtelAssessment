using Library.Service.Dto;

namespace Library.Service.Persistence
{
    public interface ILibraryRepository
    {
        Task<IEnumerable<BookDto>> GetMostBorrowedBooksAsync(int limit);
        Task<BookDetailsDto> GetBookDetailsAsync(int bookId);
        Task<int> GetBorrowedCopiesCountAsync(int bookId);
        Task<int> GetAvailableCopiesCountAsync(int bookId);
        Task<IEnumerable<UserDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int limit);
        Task<IEnumerable<BookDto>> GetBooksBorrowedByUserAsync(int userId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<BookDto>> GetRelatedBooksAsync(int bookId);
        Task<float> GetReadRateAsync(int bookId);

    }
}
