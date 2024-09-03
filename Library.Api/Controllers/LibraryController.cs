using Library.Grpc.Contract;
using Microsoft.AspNetCore.Mvc;
using static Library.Grpc.Contract.LibraryService;

namespace Library.Api.Controllers
{
    public class LibraryController : ControllerBase
    {
        private readonly LibraryServiceClient _grpcClient;

        public LibraryController(LibraryServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        [HttpGet("most-borrowed-books")]
        public async Task<IActionResult> GetMostBorrowedBooks(int limit)
        {
            var request = new MostBorrowedBooksRequest { Limit = limit };
            var response = await _grpcClient.GetMostBorrowedBooksAsync(request);
            return Ok(response);
        }

        [HttpGet("book-copy-status/{bookId}")]
        public async Task<IActionResult> GetBookCopyStatus(int bookId)
        {
            var request = new BookCopyStatusRequest { BookId = bookId };
            var response = await _grpcClient.GetBookCopyStatusAsync(request);
            return Ok(response);
        }

        [HttpGet("books-borrowed-by-user")]
        public async Task<IActionResult> GetBooksBorrowedByUser(int userId, string startDate, string endDate)
        {
            var request = new BooksBorrowedByUserRequest
            {
                UserId = userId,
                StartDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.Parse(startDate).ToUniversalTime()),
                EndDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.Parse(endDate).ToUniversalTime())
            };
            var response = await _grpcClient.GetBooksBorrowedByUserAsync(request);
            return Ok(response);
        }

        [HttpGet("related-books/{bookId}")]
        public async Task<IActionResult> GetRelatedBooks(int bookId)
        {
            var request = new RelatedBooksRequest { BookId = bookId };
            var response = await _grpcClient.GetRelatedBooksAsync(request);
            return Ok(response);
        }

        [HttpGet("top-borrowers")]
        public async Task<IActionResult> GetTopBorrowers(string startDate, string endDate)
        {
            var request = new TopBorrowersRequest
            {
                StartDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.Parse(startDate).ToUniversalTime()),
                EndDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.Parse(endDate).ToUniversalTime())
            };
            var response = await _grpcClient.GetTopBorrowersAsync(request);
            return Ok(response);
        }

        [HttpGet("read-rate/{bookId}")]
        public async Task<IActionResult> GetReadRate(int bookId)
        {
            var request = new ReadRateRequest { BookId = bookId };
            var response = await _grpcClient.GetReadRateAsync(request);
            return Ok(response);
        }
    }
}
