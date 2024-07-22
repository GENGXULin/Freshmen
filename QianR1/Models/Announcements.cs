using System;
using System.ComponentModel.DataAnnotations;

namespace COMP3851B.Models
{
    public class Announcements
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        [Required]
        public DateTime LastUpdateDate { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [MaxLength(100)]
        public string Publisher { get; set; }
    }
}
