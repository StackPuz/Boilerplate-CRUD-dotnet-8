using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public partial class UserAccount
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
        [MaxLength(100)]
        public string PasswordResetToken { get; set; }
        [Required]
        public bool Active { get; set; }
    }
}