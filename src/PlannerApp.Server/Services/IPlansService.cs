using Microsoft.EntityFrameworkCore;
using PlannerApp.Models;
using PlannerApp.Server.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlannerApp.Server.Services
{
    public interface IPlansService
    {

        IEnumerable<Plan> GetAllPlansAsync(int pageSize, int pageNumber, string userId, out int totalPlans);
        IEnumerable<Plan> SearchPlansAsync(string query, int pageSize, int pageNumber, string userId, out int totalPlans);
        Task<Plan> AddPlanAsync(string name, string description, string imagePath, string userId);
        Task<Plan> EditPlanAsync(string id, string newName, string description, string newImagePath, string userId);
        Task<Plan> DeletePlanAsync(string id, string userId);
        Task<Plan> GetPlanById(string id, string userId); 
        Plan GetPlanByName(string name, string userId); 
    }

    public class PlansService : IPlansService
    {

        private readonly ApplicationDbContext _db;
        public PlansService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Plan> AddPlanAsync(string name, string description, string imagePath, string userId)
        {
            var plan = new Plan
            {
                CoverPath = imagePath,
                Title = name,
                Description = description,
                UserId = userId,
            };

            await _db.Plans.AddAsync(plan);
            await _db.SaveChangesAsync();

            return plan;
        }

        public async Task<Plan> DeletePlanAsync(string id, string userId)
        {
            var plan = await _db.Plans.FindAsync(id);
            if (plan.UserId != userId || plan.IsDeleted)
                return null;

            plan.IsDeleted = true;
            plan.ModifiedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return plan;
        }

        public async Task<Plan> EditPlanAsync(string id, string newName, string description, string newImagePath, string userId)
        {
            var plan = await _db.Plans.FindAsync(id);
            if (plan.UserId != userId || plan.IsDeleted)
                return null;

            plan.Title = newName;
            plan.Description = description;
            if (newImagePath != null)
                plan.CoverPath = newImagePath;
            plan.ModifiedDate = DateTime.Now;

            await _db.SaveChangesAsync();
            return plan;
        }

        public IEnumerable<Plan> GetAllPlansAsync(int pageSize, int pageNumber, string userId, out int totalPlans)
        {
            // total plans 
            var allPlans = _db.Plans.Where(p => !p.IsDeleted && p.UserId == userId);
          
            totalPlans = allPlans.Count();

            var plans = allPlans.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
            foreach (var item in plans)
            {
                item.ToDoItems = _db.ToDoItems.Where(i => !i.IsDeleted && i.PlanId == item.Id).ToArray();
            }

            return plans; 
        }

        public async Task<Plan> GetPlanById(string id, string userId)
        {
            var plan = await _db.Plans.FindAsync(id);
            if (plan.UserId != userId || plan.IsDeleted)
                return null;

            plan.ToDoItems = _db.ToDoItems.Where(i => !i.IsDeleted && i.UserId == userId && i.PlanId == id).ToArray();

            return plan;
        }

        public Plan GetPlanByName(string name, string userId)
        {
            var plan = _db.Plans.SingleOrDefault(p => p.Title == name && p.UserId == userId);
            if (plan.UserId != userId || plan.IsDeleted)
                return null;

            //plan.ToDoItems = _db.ToDoItems.Where(i => !i.IsDeleted && i.UserId == userId && i.PlanId == id).ToArray();
            return plan;
        }

        public IEnumerable<Plan> SearchPlansAsync(string query, int pageSize, int pageNumber, string userId, out int totalPlans)
        {
            // total plans 
            var allPlans = _db.Plans.Where(p => !p.IsDeleted && p.UserId == userId && (p.Description.Contains(query) || p.Title.Contains(query)));

            totalPlans = allPlans.Count();

            var plans = allPlans.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
            foreach (var item in plans)
            {
                item.ToDoItems = _db.ToDoItems.Where(i => !i.IsDeleted && i.PlanId == item.Id).ToArray();
            }

            return plans;
        }


    }
}
