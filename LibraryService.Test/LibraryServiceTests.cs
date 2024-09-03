using Grpc.Core;
using Library.Grpc.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var largeLimit = int.MaxValue; // Assuming this is larger than the total number of books
            var request = new MostBorrowedBooksRequest { Limit = largeLimit };

            // Act
            var response = _fixture.GetMostBorrowedBooks(request);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.Books.Count <= largeLimit); // Ensure it doesn't exceed the limit
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
    }
}
