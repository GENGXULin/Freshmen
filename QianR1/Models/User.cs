using System.ComponentModel.DataAnnotations;

namespace COMP3851B.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        public string? Salt { get; set; }

        public string? HashedPw { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? StudentNumber { get; set; }
    }
}
