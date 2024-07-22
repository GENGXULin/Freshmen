using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class Registration
    {

        public string FirstName { get; set; }


        [Required(ErrorMessage = "LastName is required")]
        [MaxLength(50, ErrorMessage = "Max 50 char")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        //[DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = "Max 100 char")]
        //[EmailAddress( ErrorMessage = "Please Enter Vaild Email.")]

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(20, ErrorMessage = "Max 20 char")]

        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        //[DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or  min 5 char")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please comfired your password")]
        public string ComfirmPassword { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } // Role property for role selection
    }
}
