using PlannerApp.Models;
using PlannerApp.Models.V2.DTO;

namespace PlannerApp.Server.Mappers
{
    public static class ToDoItemMapper
    {
        public static ToDoItemDetail ToToDoItemDetail(this ToDoItem model)
        {
            return new ToDoItemDetail
            {
                Id = model.Id,
                Description = model.Description,
                AchievedDate = model.AchievedDate,
                EstimationDate = model.EstimatedDate,
                IsDone = model.IsDone,
                PlanId = model.PlanId
            };
        }
    }
}
