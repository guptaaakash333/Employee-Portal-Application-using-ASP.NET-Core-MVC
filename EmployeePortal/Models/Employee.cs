using System.ComponentModel.DataAnnotations;

namespace EmployeePortal.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Position is required")]
        [StringLength(50, ErrorMessage = "Position cannot be longer than 50 characters")]
        public string Position { get; set; }


        [Required(ErrorMessage = "Department is required")]
        public Department? Department { get; set; }


        [Required(ErrorMessage = "Hire Date is required")]
        [Display(Name = "Hire Date")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime? HireDate { get; set; }


        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateTime? DateOfBirth { get; set; }


        [Required(ErrorMessage = "Employee Type is required")]
        [Display(Name = "Employee Type")]
        public EmployeeType? Type { get; set; }


        [Required(ErrorMessage = "Gender is required")]
        [StringLength(6, ErrorMessage = "Gender should be Male, Female, or Other")]
        public string? Gender { get; set; }


        [Required(ErrorMessage = "Salary is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number")]
        [DataType(DataType.Currency)]
        public decimal? Salary { get; set; }
    }
}
