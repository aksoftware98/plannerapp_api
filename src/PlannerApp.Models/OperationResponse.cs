using System;

namespace PlannerApp.Models
{

    public class ApiBaseResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        
    }

    public class OperationResponse<T> : ApiBaseResponse
    {

        public OperationResponse()
        {
            OperationDate = DateTime.UtcNow;
        }

        public T Record { get; set; }
        public DateTime OperationDate { get; set; }
    }

    public class OperationResponse : ApiBaseResponse
    {

    }
}
