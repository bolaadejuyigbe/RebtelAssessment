using Grpc.Net.Client;
using Library.Grpc;
using Library.Grpc.Contract;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using static Library.Grpc.Contract.LibraryService;

namespace LibraryService.Test
{
    public class LibraryServiceFixture : GrpcTestFixture<StartupExtension>, IClassFixture<GrpcTestFixture<StartupExtension>>
    {
        private readonly LibraryServiceClient  _client;

        public LibraryServiceFixture()
        {
            _client = new LibraryServiceClient(Channel);
        }

        public MostBorrowedBooksResponse GetMostBorrowedBooks(MostBorrowedBooksRequest request)
        {
            return Execute(() => _client.GetMostBorrowedBooks(request, deadline: DateTime.UtcNow.AddSeconds(15)));
        }

        public BookCopyStatusResponse GetBookCopyStatus(BookCopyStatusRequest request)
        {
            return Execute(() => _client.GetBookCopyStatus(request, deadline: DateTime.UtcNow.AddSeconds(15)));
        }

        public BooksBorrowedByUserResponse GetBooksBorrowedByUser(BooksBorrowedByUserRequest request)
        {
            return Execute(() => _client.GetBooksBorrowedByUser(request, deadline: DateTime.UtcNow.AddSeconds(15)));
        }
        public RelatedBooksResponse GetRelatedBooks(RelatedBooksRequest request)
        {
            return Execute(() => _client.GetRelatedBooks(request, deadline: DateTime.UtcNow.AddSeconds(15)));
        }
        public TopBorrowersResponse GetTopBorrowers(TopBorrowersRequest request)
        {
            return Execute(() => _client.GetTopBorrowers(request, deadline: DateTime.UtcNow.AddSeconds(15)));
        }
        public ReadRateResponse GetReadRate(ReadRateRequest request)
        {
            return Execute(() => _client.GetReadRate(request, deadline: DateTime.UtcNow.AddSeconds(15)));
        }
    }
}
