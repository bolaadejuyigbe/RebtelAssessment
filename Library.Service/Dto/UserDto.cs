﻿
namespace Library.Service.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int BorrowedBooksCount { get; set; }
    }

}
