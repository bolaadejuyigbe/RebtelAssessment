// See https://aka.ms/new-console-template for more information
using Library.DataModel.Models;
using System;
using System.Text;

var h =IsNumber2Power(8);
PrintOddNumbers();
ReplicateString("ert", 4);
Console.WriteLine(h);

var book1 = new Book { Title = "1984", Author = "George Orwell", TotalPages = 328, TotalCopies = 5, ISBN = "978-93-5300-895-6" };
var book2 = new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", TotalPages = 281, TotalCopies = 3, ISBN = "978-45-8957-700-6" };
var book3 = new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", TotalPages = 180, TotalCopies = 4, ISBN = "978-23-4340-456-2" };
var book4 = new Book { Title = "The Trial of Brother Jero", Author = "Wole Soyinka", TotalPages = 150, TotalCopies = 7, ISBN = "978-35-5693-426-0" };
var book5 = new Book { Title = "Things Fall Apart", Author = "Chinua Achebe", TotalPages = 236, TotalCopies = 8, ISBN = "978-07-9247-242-9" };

var user1 = new User { Name = "John Doe", Email = "john.doe@example.com" };
var user2 = new User { Name = "Jane Smith", Email = "jane.smith@example.com" };
var user3 = new User { Name = "Alice Johnson", Email = "alice.johnson@example.com" };
var user4 = new User { Name = "Debola Moses", Email = "debola.moeses@example.com" };
var borrowRecord1 = new BorrowRecord { Book = book1, User = user1, BorrowedDate = DateTime.Now.AddDays(-10), ReturnedDate = DateTime.Now.AddDays(-5) };
var borrowRecord2 = new BorrowRecord { Book = book2, User = user2, BorrowedDate = DateTime.Now.AddDays(-8), ReturnedDate = DateTime.Now.AddDays(-3) };
var borrowRecord3 = new BorrowRecord { Book = book3, User = user3, BorrowedDate = DateTime.Now.AddDays(-6) };
var borrowRecord4 = new BorrowRecord { Book = book4, User = user4, BorrowedDate = DateTime.Now.AddDays(-15), ReturnedDate = DateTime.Now.AddDays(-3) };
var borrowRecord5 = new BorrowRecord { Book = book5, User = user2, BorrowedDate = DateTime.Now.AddDays(-12), ReturnedDate = DateTime.Now.AddDays(-2) };
var borrowRecord6 = new BorrowRecord { Book = book4, User = user3, BorrowedDate = DateTime.Now.AddDays(-4) };
var borrowRecord7 = new BorrowRecord { Book = book5, User = user4, BorrowedDate = DateTime.Now.AddDays(-3) };
var borrowRecord8 = new BorrowRecord { Book = book4, User = user3, BorrowedDate = DateTime.Now.AddDays(-12), ReturnedDate = DateTime.Now.AddDays(-3) };
var borrowRecord9 = new BorrowRecord { Book = book2, User = user1, BorrowedDate = DateTime.Now.AddDays(-2) };
var borrowRecord10 = new BorrowRecord { Book = book1, User = user2, BorrowedDate = DateTime.Now.AddDays(-1) };
var borrowRecords = new List<BorrowRecord>
{
    borrowRecord1,
    borrowRecord2,
    borrowRecord3,
    borrowRecord4,
    borrowRecord5,
    borrowRecord6,
    borrowRecord7,
    borrowRecord8,
    borrowRecord9,
    borrowRecord10
};

var mostBorrowedBooks = borrowRecords
    .GroupBy(br => br.Book)
    .Select(group => new
    {
        Book = group.Key,
        BorrowCount = group.Count()
    })
    .OrderByDescending(b => b.BorrowCount)
    .ToList();

// Display the list of books in order
Console.WriteLine("Books in order of most borrowed to least borrowed:");
foreach (var book in mostBorrowedBooks)
{
    Console.WriteLine($"Book: '{book.Book.Title}', Author: {book.Book.Author}, Borrow Count: {book.BorrowCount}");
}
Console.ReadKey();
static bool IsNumber2Power(int num)
{
    if (num <= 0)
    {
        return false;
    }
    return (num & (num - 1)) == 0;

}

 static void PrintOddNumbers()
{
    for (int i = 1; i < 100; i += 2)
    {
        Console.WriteLine(i);
    }
}

static string ReplicateString(string input, int times)
{
    if (times <= 0)
        return string.Empty;

    var replicatedStringBuilder = new StringBuilder(input.Length * times);

    for (int i = 0; i < times; i++)
    {
        replicatedStringBuilder.Append(input);
    }
    return replicatedStringBuilder.ToString();
}