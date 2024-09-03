using Library.Service.Dto;

namespace Library.Grpc.Services
{
    public static class LibraryExtension
    {
        public static Contract.Book ToContractBook(this BookDto bookDto) 
        {
          return new Contract.Book
          {
             Id = bookDto.Id.ToString(),
             Author = bookDto.Author,
             Title = bookDto.Title, 
             TotalPages = bookDto.TotalPages,   
             AvailableCopies = bookDto.AvailableCopies,
             BorrowedBooksCount = bookDto.BorrowedCopies,
          };
        }
        public static IEnumerable<Contract.Book> MapToGrpcBooks(IEnumerable<BookDto> bookDtos)
        {
            return bookDtos.Select(dto => ToContractBook(dto));
        }

        public static Contract.User ToContractUser(this UserDto userDto) 
        {
            return new Contract.User
            {
                BorrowedBooksCount = userDto.BorrowedBooksCount,
                Email = userDto.Email,
                Name = userDto.Name,
                Id = userDto.Id.ToString(),
            };
        }

        public static IEnumerable<Contract.User> MapToGrpcUsers(IEnumerable<UserDto> userDtos)
        {
            return userDtos.Select(dto => ToContractUser(dto));
        }
    }
}
