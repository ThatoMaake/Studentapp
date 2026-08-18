using System.ComponentModel.DataAnnotations;

namespace StudentResultApp.Models
{
    public class StudentResult
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Student number is required.")]
        [StringLength(30)]
        public string StudentNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(120)]
        public string FullName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Please select a module.")]
        public int ModuleId { get; set; }

        [Range(0, 100, ErrorMessage = "Mark must be between 0 and 100.")]
        public int Mark { get; set; }

        [StringLength(10)]
        public string Result { get; set; } = string.Empty;

        public Module? Module { get; set; }
    }
}