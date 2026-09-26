using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.DTOs
{
    public class PaggedResult<T>
    {
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public T? Data { get; set; }
        public int? StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
