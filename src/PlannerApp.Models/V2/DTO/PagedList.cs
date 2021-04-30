using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlannerApp.Models.V2.DTO
{
    public class PagedList<T> : List<T>
    {

        public int TotalPages { get; private set; }
        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public int ItemsCount { get; private set; }

        public PagedList(IEnumerable<T> data, int page, int pageSize)
        {
            PrepareData(data, page, pageSize); 
        }

        public PagedList()
        {

        }
        
        private void PrepareData(IEnumerable<T> data, int page, int pageSize)
        {
            Clear(); 
            var pageData = data.Skip((page - 1) * pageSize).Take(pageSize);
            AddRange(pageData);

            ItemsCount = data.Count();
            TotalPages = ItemsCount / PageSize;
            if ((ItemsCount % PageSize) > 0)
                TotalPages++; 

        }
    }
}
