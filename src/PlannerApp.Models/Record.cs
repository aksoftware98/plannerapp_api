using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace PlannerApp.Models
{
    public class Record
    {

        public Record()
        {
            Id = Guid.NewGuid().ToString();
            CreatedDate = DateTime.UtcNow;
            ModifiedDate = DateTime.UtcNow; 
        }

        [Key]
        public string Id { get; set; }

        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public DateTime ModifiedDate { get; set; }

        [Required]
        [StringLength(40)]
        [JsonIgnore]
        public string UserId { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }
    }
}
