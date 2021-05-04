using Microsoft.EntityFrameworkCore;
using PlannerApp.Models;
using PlannerApp.Models.V2.DTO;
using PlannerApp.Server.Data;
using PlannerApp.Server.Exceptions;
using PlannerApp.Server.Interfaces;
using PlannerApp.Server.Mappers;
using PlannerApp.Server.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.Server.Services.V2
{
    public class ToDosService : IToDosService
    {

        private readonly ApplicationDbContext _db;
        private readonly IdentityOptions _identity;

        public ToDosService(ApplicationDbContext db, IdentityOptions identity)
        {
            _db = db;
            _identity = identity;
        }

        public async Task<ToDoItemDetail> CreateAsync(ToDoItemDetail item)
        {
            var plan = await _db.Plans.FindAsync(item.PlanId);
            if (plan == null)
                throw new NotFoundException($"Plan with the {item.PlanId} couldn't be found");

            var todoItem = new ToDoItem
            {
                EstimatedDate = item.EstimationDate,
                CreatedDate = DateTime.UtcNow,
                Description = item.Description,
                Id = Guid.NewGuid().ToString(),
                IsDeleted = false,
                IsDone = false,
                Plan = plan,
                UserId = _identity.UserId,
                ModifiedDate = DateTime.UtcNow,
            };
            await _db.ToDoItems.AddAsync(todoItem);
            await _db.SaveChangesAsync();

            return todoItem.ToToDoItemDetail(); 
        }

        public async Task DeleteAsync(string id)
        {
            var item = await _db.ToDoItems.FindAsync(id);
            if (item == null)
                throw new NotFoundException($"ToDo with the {id} couldn't be found");

            item.IsDeleted = true;
            item.ModifiedDate = DateTime.UtcNow;     
            await _db.SaveChangesAsync(); 
        }

        public async Task<ToDoItemDetail> EditAsync(ToDoItemDetail model)
        {
            var item = await _db.ToDoItems.FindAsync(model.Id);
            if (item == null)
                throw new NotFoundException($"ToDo with the {model.Id} couldn't be found");

            item.Description = model.Description;
            item.AchievedDate = model.AchievedDate;
            item.ModifiedDate = DateTime.UtcNow;     
            item.IsDone = model.IsDone;
            
            await _db.SaveChangesAsync();
            return item.ToToDoItemDetail();
        }

        public async Task<PagedList<ToDoItemDetail>> GetNotdoneAsync(int page = 1, int pageSize = 12)
        {
            if (page < 1)
                page = 1;
            if (pageSize < 5)
                pageSize = 5;
            if (pageSize > 50)
                pageSize = 50;

            var notDoneItems = await (from i in _db.ToDoItems
                                      where i.UserId == _identity.UserId
                                      && !i.IsDeleted && !i.IsDone
                                      orderby i.CreatedDate descending
                                      select i).ToArrayAsync();

            var pagedList = new PagedList<ToDoItemDetail>(notDoneItems.Select(i => i.ToToDoItemDetail()), page, pageSize);
            return pagedList; 
        }

        public async Task ToggleItemAsync(string id)
        {
            var item = await _db.ToDoItems.FindAsync(id);
            if (item == null)
                throw new NotFoundException($"ToDo with the {id} couldn't be found");
            
            if (item.IsDone)
            {
                item.IsDone = false;
                item.AchievedDate = null; 
            }
            else
            {
                item.IsDone = true;
                item.AchievedDate = DateTime.UtcNow;
            }
            item.ModifiedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
    }
}
