using System;
using System.Collections.Generic;
using System.Text;

namespace PlannerApp.Models.V2.Responses
{
    public class ApiResponse : ApiBaseResponse
    {
        public ApiResponse(string message)
        {
            Message = message;
        }

        public ApiResponse()
        {

        }
    }

    public class ApiResponse<T> : ApiBaseResponse
    {
        public T Value { get; set; }

        public ApiResponse()
        {
            IsSuccess = true;
        }

        public ApiResponse(T value)
        {
            IsSuccess = true;
            Value = value; 
        }

        public ApiResponse(T value, string message)
        {
            IsSuccess = true;
            Value = value;
            Message = message; 
        }

    }
}
