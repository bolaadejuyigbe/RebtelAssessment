using Grpc.Net.Client;
using Library.Grpc.Contract;
using static Library.Grpc.Contract.LibraryService;

namespace LibraryService.Test
{
    public class LibraryServiceFixture : GrpcTestFixture, IDisposable
    {
        private readonly GrpcChannel _channel;
        private readonly LibraryServiceClient  _client;
        public LibraryServiceFixture()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            _channel = GrpcChannel.ForAddress("http://localhost:5000", new GrpcChannelOptions
            {
                MaxReceiveMessageSize = 10 * 1024 * 1024 // 10MB
            });

            _client = new LibraryServiceClient(_channel);
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
        // Dispose of the channel when done
        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
