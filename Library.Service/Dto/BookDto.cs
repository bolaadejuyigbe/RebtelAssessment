using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Service.Dto
{
    public class BookDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int TotalPages { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string? ISBN { get; set; }
    }
}
