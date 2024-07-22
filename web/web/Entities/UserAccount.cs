using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using web.Models;

namespace web.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class UserAccount
    {



        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "FirstName is required")]
        [MaxLength(50, ErrorMessage = "Max 50 char")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "LastName is required")]
        [MaxLength(50, ErrorMessage = "Max 50 char")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        //[DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = "Max 100 char")]
        public string Email { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(20, ErrorMessage = "Max 20 char")]

        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        //[DataType(DataType.Password)]
        [MaxLength(20, ErrorMessage = "Max 20 char")]
        public string Password { get; set; }



        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; } // defult is User




    }
}
