using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository
{
    public class PostSpecParams
    {
        private const int maxSize = 10;
        private int pageIndex = 1;
        private int pageSize = 5;
        public int PageIndex 
        {
            get
            {
                return pageIndex;
            }
            set
            {
                if (value <= 0)
                    pageIndex = 1;
                else
                    pageIndex = value;
            }
        }
        public int PageSize 
        {   get 
            {
                return pageSize;
            }
            set
            { 
                if(value <= 0)
                    pageSize = 5;
                else if (value > maxSize)
                    pageSize = maxSize;
                else
                    pageSize = value;
            }
        }
        public string? AuthorName { get; set; }
        public DateTime? DateSearch { get; set; }

    }
}
