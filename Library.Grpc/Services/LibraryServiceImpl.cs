using Azure;
using Grpc.Core;
using Library.Grpc.Contract;
using Library.Service.Dto;
using Library.Service.Services;

namespace Library.Grpc.Services
{
    public class LibraryServiceImpl : Contract.LibraryService.LibraryServiceBase
    {
        private readonly ILibraryService _libraryService;
        private readonly ILogger<LibraryServiceImpl> _logger;

        public LibraryServiceImpl(ILibraryService libraryService, ILogger<LibraryServiceImpl> logger)
        {
            _libraryService = libraryService;
            _logger = logger;
        }

        public override async Task<MostBorrowedBooksResponse> GetMostBorrowedBooks(MostBorrowedBooksRequest request, ServerCallContext context)
        {
            var bookDtos = await _libraryService.GetMostBorrowedBooksAsync(request.Limit, context.CancellationToken).ConfigureAwait(false);
            var response = new MostBorrowedBooksResponse();
            response.Books.AddRange(LibraryExtension.MapToGrpcBooks(bookDtos));
            return response;
        }

        public override async Task<BookCopyStatusResponse> GetBookCopyStatus(BookCopyStatusRequest request, ServerCallContext context)
        {
            var bookStatus = await _libraryService.GetBookStatusAsync(request.BookId, context.CancellationToken).ConfigureAwait(false);

            return new BookCopyStatusResponse
            {
                BorrowedCopies = bookStatus.BorrowedBooksCount,
                AvailableCopies = bookStatus.AvailableCopies,
                Title = bookStatus.Title,
                TotalCopies = bookStatus.TotalCopies.ToString()   
            };
        }

        public override async Task<BooksBorrowedByUserResponse> GetBooksBorrowedByUser(BooksBorrowedByUserRequest request, ServerCallContext context)
        {
          var borrowedBooks = await _libraryService.GetBooksBorrowedByUserAsync(
          request.UserId,
          request.StartDate.ToDateTime(),
          request.EndDate.ToDateTime(),
          context.CancellationToken
           ).ConfigureAwait(false);

            var response = new BooksBorrowedByUserResponse();
            response.Books.AddRange(LibraryExtension.MapToGrpcBooks(borrowedBooks));
            return response;
        }

        public override async Task<RelatedBooksResponse> GetRelatedBooks(RelatedBooksRequest request, ServerCallContext context)
        {
            var relatedBooks = await _libraryService.GetRelatedBooksAsync(request.BookId, context.CancellationToken).ConfigureAwait(false);
            var response = new RelatedBooksResponse();
            response.Books.AddRange(LibraryExtension.MapToGrpcBooks(relatedBooks));
            return response;
        }

        public override async Task<TopBorrowersResponse> GetTopBorrowers(TopBorrowersRequest request, ServerCallContext context)
        {
            var topBorrowers = await _libraryService.GetTopBorrowersAsync(
                request.StartDate.ToDateTime(),
                request.EndDate.ToDateTime(),
                10,
                context.CancellationToken).ConfigureAwait(false);
            var response = new TopBorrowersResponse();
            response.Users.AddRange(LibraryExtension.MapToGrpcUsers(topBorrowers));

            return response;
        }

        public override async Task<ReadRateResponse> GetReadRate(ReadRateRequest request, ServerCallContext context)
        {
            var readRate = await _libraryService.GetReadRateAsync(request.BookId, context.CancellationToken);
            var response = new ReadRateResponse
            {
                PagesPerDay = readRate
            };
            return response; 
        }
    }
}
