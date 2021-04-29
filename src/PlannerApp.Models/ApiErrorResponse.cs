using System;
using System.Collections.Generic;
using System.Text;

namespace PlannerApp.Models
{
    public class ApiErrorResponse
    {

        public string Message { get; set; }

        public string[] Errors { get; set; }

        public bool IsSuccess { get; set; }

        public ApiErrorResponse()
        {

        }

        public ApiErrorResponse(string message)
        {
            Message = message;
            IsSuccess = false; 
        }

        public ApiErrorResponse(string message, string[] errors)
        {
            IsSuccess = false;
            Message = message;
            Errors = errors; 
        }

    }
}
