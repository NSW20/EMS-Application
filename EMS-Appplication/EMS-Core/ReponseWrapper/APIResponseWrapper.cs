using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS_Core.ReponseWrapper
{
    public class APIResponseWrapper<T> 
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public static APIResponseWrapper<T> Ok(int statusCode,T data, string message = "Success")
        {
             return new() { StatusCode = statusCode, Message = message, Data = data };
        }
        public static APIResponseWrapper<T> Fail(int statusCode, string message, List<string>? errors = null)
        {
            return new APIResponseWrapper<T>() { Message = message, StatusCode = statusCode, Errors = errors };
        }

    }
}
