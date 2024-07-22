using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web.Models
{
    public class Login
    {
        [Required(ErrorMessage = "UserName or Email is required")]
        [MaxLength(20, ErrorMessage = "Max 20 char")]
        [DisplayName("UserName or Email")]

        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        //[DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or  min 5 char")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
