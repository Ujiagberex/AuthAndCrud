using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebApplication1.Models
{
    public class Student
    {
        [Required]
        [Key]
        public int StudentId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(6)]
        public string Gender { get; set; }
        public int? Age { get; set; }
        [StringLength(20)]
        public string? Cohort { get; set; }

    }
}
