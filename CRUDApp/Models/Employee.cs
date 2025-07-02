using System.ComponentModel.DataAnnotations;

namespace CRUDApp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
    }
}
