using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlannerApp.Models
{

    public class ToDoItem : Record
    {

        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        
        public bool IsDone { get; set; }
        public DateTime? EstimatedDate { get; set; }
        public DateTime? AchievedDate { get; set; } 

        public virtual Plan Plan { get; set; }

        [ForeignKey("Plan")]
        public string PlanId { get; set; }
    }
}
