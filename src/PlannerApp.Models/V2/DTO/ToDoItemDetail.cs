using System;
using System.ComponentModel.DataAnnotations;

namespace PlannerApp.Models.V2.DTO
{
    public class ToDoItemDetail
    {
        public string Id { get; set; }

        [Required]
        public string Description { get; set; }
        public DateTime? EstimationDate { get; set; }
        public DateTime? AchievedDate { get; set; }
        public bool IsDone { get; set; }

        [Required]
        [StringLength(50)]
        public string PlanId { get; set; }
    }
}
