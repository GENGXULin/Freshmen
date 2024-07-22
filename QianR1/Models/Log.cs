using System;
using System.ComponentModel.DataAnnotations;

namespace COMP3851B.Models
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        public string LogMessage { get; set; }

        public DateTime LogDate { get; set; } = DateTime.Now;

        [StringLength(50)] // 设置最大长度为50
        public string Number { get; set; }
    }
}
