using PlannerApp.Models.V2.DTO;
using System.Threading.Tasks;

namespace PlannerApp.Server.Interfaces
{
    public interface IToDosService
    {
        Task<ToDoItemDetail> CreatAsync(ToDoItemDetail item);
        Task<ToDoItemDetail> EditAsync(ToDoItemDetail plan);
        Task DeleteAsync(string id);
        Task ToggleItemAsync(string id);
        Task<PagedList<ToDoItemDetail>> GetNotdoneAsync(int page = 1, int pageSize = 12);
    }
}
