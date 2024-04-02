using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public partial class OrderHeader
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
    }
}