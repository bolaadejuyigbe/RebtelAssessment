using Azure.Core;
using Library.Service.Dto;
using Library.Service.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Service.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository _libraryRepository;

        public LibraryService(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository; 
        }
        public async Task<IEnumerable<BookDto>> GetMostBorrowedBooksAsync(int limit, CancellationToken cancellationToken)
        {
            return await _libraryRepository.GetMostBorrowedBooksAsync(limit).ConfigureAwait(false);
        }

        public async Task<IEnumerable<UserDto>> GetTopBorrowersAsync(DateTime startDate, DateTime endDate, int limit, CancellationToken cancellationToken)
        {
            return await _libraryRepository.GetTopBorrowersAsync(startDate, endDate, limit).ConfigureAwait(false);
        }

        public async Task<IEnumerable<BookDto>> GetBooksBorrowedByUserAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            return await _libraryRepository.GetBooksBorrowedByUserAsync(userId, startDate, endDate).ConfigureAwait(false);
        }

        public async Task<IEnumerable<BookDto>> GetRelatedBooksAsync(int bookId, CancellationToken cancellationToken)
        {
            return await _libraryRepository.GetRelatedBooksAsync(bookId).ConfigureAwait(false);
        }

        public async Task<BookStatusDto> GetBookStatusAsync(int bookId, CancellationToken cancellationToken)
        {
            var borrowedCopiesCount = await _libraryRepository.GetBorrowedCopiesCountAsync(bookId).ConfigureAwait(false);

            var availableCopiesCount = await _libraryRepository.GetAvailableCopiesCountAsync(bookId).ConfigureAwait(false);

            var bookDetails = await _libraryRepository.GetBookDetailsAsync(bookId).ConfigureAwait(false);

            return new BookStatusDto
            {
                AvailableCopies = availableCopiesCount,
                BorrowedBooksCount = borrowedCopiesCount,
                Title = bookDetails.Title,
                TotalCopies = bookDetails.TotalCopies
            };

        }

        public async Task<float> GetReadRateAsync(int bookId, CancellationToken cancellationToken)
        {
           return await _libraryRepository.GetReadRateAsync(bookId).ConfigureAwait(false);  
        }
    }
}
