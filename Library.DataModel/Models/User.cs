using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataModel.Models
{
    public class User
    {
        public int Id { get; set; }         
        public string? Name { get; set; }      
        public string? Email { get; set; }     

        // Navigation property for related borrow records
        public ICollection<BorrowRecord>? BorrowRecords { get; set; }
    }
}
