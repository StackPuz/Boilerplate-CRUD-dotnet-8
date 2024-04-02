using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public partial class Product
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int BrandId { get; set; }
        [MaxLength(50)]
        public string Image { get; set; }
        public int? CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}