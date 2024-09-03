using Library.Grpc.Contract;
using Microsoft.AspNetCore.Mvc;
using static Library.Grpc.Contract.LibraryService;
using Google.Protobuf.WellKnownTypes;
using Library.Api.Policies;

namespace Library.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly ResilientPolicy _resilientPolicy;
        public LibraryController(ResilientPolicy resilientPolicy)
        {
           _resilientPolicy = resilientPolicy;
        }

        [HttpGet("most-borrowed-books")]
        public async Task<IActionResult> GetMostBorrowedBooks(int limit)
        {
            var request = new MostBorrowedBooksRequest { Limit = limit };
            var response = await _resilientPolicy.ExecuteGetMostBorrowedBooksAsync(request);
            return Ok(response);
        }

        [HttpGet("book-copy-status/{bookId}")]
        public async Task<IActionResult> GetBookCopyStatus(int bookId)
        {
            var request = new BookCopyStatusRequest { BookId = bookId };
            var response = await _resilientPolicy.ExecuteGetBookCopyStatusAsync(request);
            return Ok(response);
        }

        [HttpGet("books-borrowed-by-user")]
        public async Task<IActionResult> GetBooksBorrowedByUser(int userId, DateTime startDate, DateTime endDate)
        {
            var request = new BooksBorrowedByUserRequest
            {
                UserId = userId,
                StartDate = Timestamp.FromDateTime(startDate.ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(endDate.ToUniversalTime())
            };
            var response = await _resilientPolicy.ExecuteGetBooksBorrowedByUserAsync(request); ;
            return Ok(response);
        }

        [HttpGet("related-books/{bookId}")]
        public async Task<IActionResult> GetRelatedBooks(int bookId)
        {
            var request = new RelatedBooksRequest { BookId = bookId };
            var response = await _resilientPolicy.ExecuteGetRelatedBooksAsync(request);
            return Ok(response);
        }

        [HttpGet("top-borrowers")]
        public async Task<IActionResult> GetTopBorrowers(DateTime startDate, DateTime endDate)
        {
            var request = new TopBorrowersRequest
            {
                StartDate = Timestamp.FromDateTime(startDate.ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(endDate.ToUniversalTime())
            };
            var response = await _resilientPolicy.ExecuteGetTopBorrowersAsync(request);
            return Ok(response);
        }

        [HttpGet("read-rate/{bookId}")]
        public async Task<IActionResult> GetReadRate(int bookId)
        {
            var request = new ReadRateRequest { BookId = bookId };
            var response = await _resilientPolicy.ExecuteGetReadRateAsync(request);
            return Ok(response);
        }
    }
}
