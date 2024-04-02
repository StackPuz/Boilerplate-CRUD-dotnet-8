using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public partial class Brand
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}