using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.ReponseWrapper
{
    public class APIResponseWrapperPagginated<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public int TotalPage { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
    }
}
