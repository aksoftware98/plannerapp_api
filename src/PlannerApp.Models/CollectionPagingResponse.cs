using System;
using System.Collections.Generic;

namespace PlannerApp.Models
{

    public class CollectionResponse<T>
    {
        public CollectionResponse()
        {
            OperationDate = DateTime.Now;
        }

        public IEnumerable<T> Records { get; set; }

        public string Message { get; set; }
        public int Count { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime OperationDate { get; set; }
    }

    public class CollectionPagingResponse<T> : CollectionResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int? NextPage { get; set; }
      
    }
}
