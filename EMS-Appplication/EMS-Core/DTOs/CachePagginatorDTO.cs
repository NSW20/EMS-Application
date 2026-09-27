using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.DTOs
{
    public class CachePagginatorDTO<T>
    {
        public IEnumerable<T>? Data { get; set; }
        public int TotalItemsCount { get; set; }
    }
}
