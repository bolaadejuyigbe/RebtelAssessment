
namespace Library.Service.Dto
{
    public class BookStatusDto
    {
        public int AvailableCopies { get; set; } = 0;
        public string? Title { get; set; }
        public int BorrowedBooksCount { get; set; }
        public int TotalCopies { get; set; }
    }
}
