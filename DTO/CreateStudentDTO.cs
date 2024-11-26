using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO
{
    public class CreateStudentDTO
    {
        public string Name { get; set; }
        [StringLength(6)]
        public string Gender { get; set; }
        public int? Age { get; set; }
        public string? Cohort { get; set; }
    }
}
