using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlannerApp.Models.V2.DTO
{
    public class PlanDetail
    {

        public string Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }


        public IFormFile CoverFile { get; set; }

        public string CoverUrl { get; set; }

        public List<ToDoItemDetail> ToDoItems { get; set; }

    }
}
