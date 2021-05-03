using PlannerApp.Models;
using PlannerApp.Models.V2.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.Server.Mappers
{
    public static class PlanMapper
    {

        public static PlanDetail ToPlanDetail(this Plan model, bool withDetails)
        {
            return new PlanDetail
            {
                Id = model.Id,
                CoverUrl = model.CoverPath,
                Description = model.Description,
                Title = model.Title,
                ToDoItems = withDetails ? model.ToDoItems?.Select(t => t.ToToDoItemDetail()).ToList() : null
            };
        }

    }
}
