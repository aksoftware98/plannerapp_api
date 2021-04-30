using PlannerApp.Models;
using PlannerApp.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.Server.Services
{
    public interface IItemsService
    {

        IEnumerable<ToDoItem> GetAllItems(string planId, string userId);

        IEnumerable<ToDoItem> GetNotAchievedItems(string userId);

        Task<ToDoItem> CreateItemAsync(string planId, string description, DateTime? estimatedDate, string userId);

        Task<ToDoItem> EditItemsAsync(string itemId, string newDescritption, DateTime? estimatedDate, string userId);

        Task<ToDoItem> MarkItemAsync(string itemId, string userId);

        Task<ToDoItem> DeleteItemAsync(string itemId, string userId);

    }

    public class ToDoItemsService : IItemsService
    {

        private readonly ApplicationDbContext _db;

        public ToDoItemsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ToDoItem> CreateItemAsync(string planId, string description, DateTime? estimatedDate, string userId)
        {
            // Check the plan if existing 
            var plan = await _db.Plans.FindAsync(planId);
            if (plan == null)
                return null;

            var item = new ToDoItem
            {
                Description = description,
                EstimatedDate = estimatedDate,
                IsDone = false,
                PlanId = planId,
                UserId = userId
            };

            await _db.ToDoItems.AddAsync(item);
            await _db.SaveChangesAsync();

            return item; 
        }

        public async Task<ToDoItem> DeleteItemAsync(string itemId, string userId)
        {
            var item = await _db.ToDoItems.FindAsync(itemId);
            if (item == null || userId != item.UserId)
                return null;

            item.IsDeleted = true;
            item.ModifiedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return item; 
        }

        public async Task<ToDoItem> EditItemsAsync(string itemId, string newDescritption, DateTime? estimatedDate, string userId)
        {
            var item = await _db.ToDoItems.FindAsync(itemId);

            if (item == null || userId != item.UserId || item.IsDone)
                return null;

            item.Description = newDescritption;
            item.ModifiedDate = DateTime.UtcNow;
            item.EstimatedDate = estimatedDate;

            await _db.SaveChangesAsync();

            return item;
        }

        public IEnumerable<ToDoItem> GetAllItems(string planId, string userId)
        {
            var items = _db.ToDoItems.Where(i => i.PlanId == planId && !i.IsDeleted && i.UserId == userId).ToArray();

            return items; 
        }

        public IEnumerable<ToDoItem> GetNotAchievedItems(string userId)
        {
            var items = _db.ToDoItems.Where(i => !i.IsDone && !i.IsDeleted && i.UserId == userId).ToArray();

            return items;
        }

        public async Task<ToDoItem> MarkItemAsync(string itemId, string userId)
        {
            var item = await _db.ToDoItems.FindAsync(itemId);

            if (item == null || userId != item.UserId)
                return null;

            item.IsDone = !item.IsDone;
            item.ModifiedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return item;
        }
    }
}
