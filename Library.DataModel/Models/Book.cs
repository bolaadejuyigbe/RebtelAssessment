using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataModel.Models
{
    public class Book
    {
        public int Id { get; set; }          
        public string? Title { get; set; }     
        public string? Author { get; set; }   
        public required int TotalPages { get; set; }   
        public required int TotalCopies { get; set; }  
        public required string ISBN { get; set; }

        // Navigation property for related borrow records
        public ICollection<BorrowRecord>? BorrowRecords { get; set; }
    }
}
