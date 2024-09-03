using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Library.Grpc.Contract;

namespace LibraryService.Test
{
    public class LibraryServiceTests: IClassFixture<LibraryServiceFixture>
    {
        private readonly LibraryServiceFixture _fixture;

        public LibraryServiceTests(LibraryServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetMostBorrowedBooks_ReturnsExpectedResult()
        {
            // Arrange
            var request = new MostBorrowedBooksRequest { Limit = 10 };

            // Act
            var response = _fixture.GetMostBorrowedBooks(request);

            // Assert
            Assert.NotNull(response);
            Assert.NotEmpty(response.Books);
        }

        [Fact]
        public void GetMostBorrowedBooks_WithZeroLimit_ReturnsEmptyResult()
        {
            // Arrange
            var request = new MostBorrowedBooksRequest { Limit = 0 };

            // Act
            var response = _fixture.GetMostBorrowedBooks(request);

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response.Books);
        }

        [Fact]
        public void GetMostBorrowedBooks_WithNegativeLimit_ThrowsArgumentException()
        {
            // Arrange
            var request = new MostBorrowedBooksRequest { Limit = -1 };

            // Act & Assert
            Assert.Throws<RpcException>(() => _fixture.GetMostBorrowedBooks(request));
        }

        [Fact]
        public void GetMostBorrowedBooks_WithLargeLimit_ReturnsAllBooks()
        {
            // Arrange
            var largeLimit = int.MaxValue; 
            var request = new MostBorrowedBooksRequest { Limit = largeLimit };

            // Act
            var response = _fixture.GetMostBorrowedBooks(request);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Books.Count <= largeLimit); 
        }

        [Fact]
        public void GetBookCopyStatus_ReturnsCorrectStatus()
        {
            // Arrange
            var request = new BookCopyStatusRequest { BookId = 1 };

            // Act
            var response = _fixture.GetBookCopyStatus(request);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.BorrowedCopies >= 0);
            Assert.True(response.AvailableCopies >= 0);
        }

        [Fact]
        public void GetBookCopyStatus_NonExistentBook_ReturnsNullOrThrows()
        {
            // Arrange
            var request = new BookCopyStatusRequest { BookId = 9999 }; 

            // Act & Assert
            var exception = Assert.Throws<RpcException>(() => _fixture.GetBookCopyStatus(request));
            Assert.Equal(StatusCode.Internal, exception.StatusCode);
        }
        [Fact]
        public void GetBookCopyStatus_MixedCopies_ReturnsCorrectStatus()
        {
            // Arrange
            var request = new BookCopyStatusRequest { BookId = 4 }; 
            // Act
            var response = _fixture.GetBookCopyStatus(request);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.BorrowedCopies > 0);
            Assert.True(response.AvailableCopies > 0);
        }
        [Fact]
        public void GetBooksBorrowedByUser_ReturnsExpectedBooks()
        {
            // Arrange
            var request = new BooksBorrowedByUserRequest
            {
                UserId = 1,
                StartDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow.AddMonths(-1)),
                EndDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow)
            };

            // Act
            var response = _fixture.GetBooksBorrowedByUser(request);

            // Assert
            Assert.NotNull(response);
            Assert.NotEmpty(response.Books);
        }

        [Fact]
        public void GetBooksBorrowedByUser_UserWithBorrowedBooks_ReturnsListOfBooks()
        {
            // Arrange
            var request = new BooksBorrowedByUserRequest
            {
                UserId = 2,
                StartDate = Timestamp.FromDateTime(DateTime.UtcNow.AddMonths(-1).ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(DateTime.UtcNow.ToUniversalTime())
            };

            // Act
            var response =  _fixture.GetBooksBorrowedByUser(request);

            // Assert
            Assert.NotNull(response);
            Assert.NotEmpty(response.Books);
            Assert.All(response.Books, book => Assert.NotNull(book.Title)); 
        }

        [Fact]
        public void GetRelatedBooks_ValidBookId_ReturnsRelatedBooks()
        {
            // Arrange
            var request = new RelatedBooksRequest { BookId = 1 }; 

            // Act
            var response = _fixture.GetRelatedBooks(request);

            // Assert
            Assert.NotNull(response);
            Assert.NotEmpty(response.Books);
        }
        [Fact]
        public void GetRelatedBooks_NonExistentBook_ReturnsEmptyList()
        {
            // Arrange
            var request = new RelatedBooksRequest { BookId = 9999 }; 

            // Act
            var response = _fixture.GetRelatedBooks(request);

            // Assert
            Assert.NotNull(response);
            Assert.Empty(response.Books);
        }

        [Fact]
        public void GetTopBorrowers_ValidTimeFrame_ReturnsTopBorrowers()
        {
            // Arrange
            var request = new TopBorrowersRequest
            {
                StartDate = Timestamp.FromDateTime(DateTime.UtcNow.AddMonths(-1).ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(DateTime.UtcNow.ToUniversalTime())
            };

            // Act
            var response = _fixture.GetTopBorrowers(request);

            // Assert
            Assert.NotNull(response);
            Assert.NotEmpty(response.Users);
        }
        [Fact]
        public void GetTopBorrowers_NoBorrowersInTimeFrame_ReturnsEmptyList()
        {
            // Arrange
            var request = new TopBorrowersRequest
            {
                StartDate = Timestamp.FromDateTime(DateTime.UtcNow.AddYears(-1).ToUniversalTime()),
                EndDate = Timestamp.FromDateTime(DateTime.UtcNow.AddMonths(-11).ToUniversalTime())
            };

            // Act
            var response = _fixture.GetTopBorrowers(request);

            // Assert
            Assert.NotNull(response.Users);
            Assert.Equal(0, response.Users.Select(x => x.BorrowedBooksCount).FirstOrDefault());
        }


    }
}
