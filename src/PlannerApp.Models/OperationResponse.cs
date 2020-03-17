using System;

namespace PlannerApp.Models
{
    public class OperationResponse<T>
    {

        public OperationResponse()
        {
            OperationDate = DateTime.UtcNow;
        }

        public T Record { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime OperationDate { get; set; }
    }
}
