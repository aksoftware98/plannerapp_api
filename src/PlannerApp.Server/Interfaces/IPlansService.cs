using PlannerApp.Models.V2.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.Server.Interfaces
{

    public interface IPlansService
    {

        Task<PlanDetail> CreateAsync(PlanDetail plan);
        Task<PlanDetail> EditAsync(PlanDetail plan);
        Task DeleteAsync(string id);
        Task<PlanDetail> GetByIdAsync(string id);
        Task<PagedList<PlanDetail>> GetPlansAsync(string query, int page = 1, int pageSize = 12);

    }


}
