using System;

namespace PlannerApp.Server.Exceptions
{
    public class ValidationException : Exception
    {

        public string[] Errors { get; set; }

        public ValidationException(string message, string[] errors) : base(message)
        {
            Errors = errors; 
        }
    }
}
