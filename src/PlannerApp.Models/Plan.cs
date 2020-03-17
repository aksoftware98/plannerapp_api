using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlannerApp.Models
{
    public class Plan : Record
    {

        public Plan()
        {
            ToDoItems = new List<ToDoItem>();
        }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [StringLength(256)]
        public string CoverPath { get; set; }

        public ICollection<ToDoItem> ToDoItems { get; set; }
    }
}
