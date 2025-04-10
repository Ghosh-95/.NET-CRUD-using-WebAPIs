using Microsoft.Build.Framework;

namespace CRUD_App_using_Web_API.Models
{

    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public int Class { get; set; } // Renamed property to 'Class' with PascalCase
        [Required]
        public string Section { get; set; }
    }
}
