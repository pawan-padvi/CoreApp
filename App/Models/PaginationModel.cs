using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class PaginationModel
    {
        public int TotalCount { get; set; }
        public int currentPage { get; set; }
        public string searchString { get; set; }
        public int pageSize { get; set; }
        public string sortCol { get; set; }
        public string sortOrder { get; set; }
    }
}
