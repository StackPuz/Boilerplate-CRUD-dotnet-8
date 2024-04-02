using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public partial class UserRole
    {
        [Key, Column(Order=1)]
        [Required]
        public int UserId { get; set; }
        [Key, Column(Order=2)]
        [Required]
        public int RoleId { get; set; }
    }
}