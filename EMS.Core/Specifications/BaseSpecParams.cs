using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Core.Specifications
{
    public class BaseSpecParams
    {
            private string? search;

            public string? Search
            {
                get => search;
                set => search = value?.ToLower();
            }


            public string? Sort { get; set; }
            private const int MaxPageSize = 50; 
            private int pageSize = 10;          

            public int PageSize
            {
                get => pageSize;
                set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }

            public int PageIndex { get; set; } = 1;

    }
}
