using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlannerApp.Models
{
    public class PlanRequest
    {

        public string Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Title { get; set; }
        
        [StringLength(200)]
        public string Description { get; set; }

        public IFormFile CoverFile { get; set; }
    }

    public class ToDoItemRequest
    {
        public string Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Description { get; set; }

        public DateTime? EstimatedDate { get; set; }

        [Required]
        public string PlanId { get; set; }
    }
}
