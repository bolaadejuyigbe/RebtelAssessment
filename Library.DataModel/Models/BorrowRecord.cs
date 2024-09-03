using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataModel.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }        
        public int BookId { get; set; }     
        public int UserId { get; set; }      
        public DateTime BorrowedDate { get; set; }  
        public DateTime? ReturnedDate { get; set; } 

        public Book? Book { get; set; }      
        public User? User { get; set; }
    }
}
