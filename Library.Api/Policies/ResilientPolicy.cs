using Library.Grpc.Contract;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace Library.Api.Policies
{
    public class ResilientPolicy
    {
        private readonly LibraryService.LibraryServiceClient _client;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private readonly AsyncTimeoutPolicy _timeoutPolicy;

        public ResilientPolicy(LibraryService.LibraryServiceClient client)
        {
            _client = client;

            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));

            _timeoutPolicy = Policy
                .TimeoutAsync(5, TimeoutStrategy.Pessimistic);
        }
        public async Task<MostBorrowedBooksResponse> ExecuteGetMostBorrowedBooksAsync(MostBorrowedBooksRequest request)
        {
            return await _retryPolicy
                .WrapAsync(_circuitBreakerPolicy)
                .WrapAsync(_timeoutPolicy)
                .ExecuteAsync(async () =>
                {
                    return await _client.GetMostBorrowedBooksAsync(request);
                });
        }
        public async Task<BookCopyStatusResponse> ExecuteGetBookCopyStatusAsync(BookCopyStatusRequest request)
        {
            return await _retryPolicy
                .WrapAsync(_circuitBreakerPolicy)
                .WrapAsync(_timeoutPolicy)
                .ExecuteAsync(async () =>
                {
                    return await _client.GetBookCopyStatusAsync(request);
                });
        }
        public async Task<BooksBorrowedByUserResponse> ExecuteGetBooksBorrowedByUserAsync(BooksBorrowedByUserRequest request)
        {
            return await _retryPolicy
                .WrapAsync(_circuitBreakerPolicy)
                .WrapAsync(_timeoutPolicy)
                .ExecuteAsync(async () =>
                {
                    return await _client.GetBooksBorrowedByUserAsync(request);
                });
        }
        public async Task<RelatedBooksResponse> ExecuteGetRelatedBooksAsync(RelatedBooksRequest request)
        {
            return await _retryPolicy
                .WrapAsync(_circuitBreakerPolicy)
                .WrapAsync(_timeoutPolicy)
                .ExecuteAsync(async () =>
                {
                    return await _client.GetRelatedBooksAsync(request);
                });
        }
        public async Task<TopBorrowersResponse> ExecuteGetTopBorrowersAsync(TopBorrowersRequest request)
        {
            return await _retryPolicy
                .WrapAsync(_circuitBreakerPolicy)
                .WrapAsync(_timeoutPolicy)
                .ExecuteAsync(async () =>
                {
                    return await _client.GetTopBorrowersAsync(request);
                });
        }
        public async Task<ReadRateResponse> ExecuteGetReadRateAsync(ReadRateRequest request)
        {
            return await _retryPolicy
                .WrapAsync(_circuitBreakerPolicy)
                .WrapAsync(_timeoutPolicy)
                .ExecuteAsync(async () =>
                {
                    return await _client.GetReadRateAsync(request);
                });
        }
    }
}
