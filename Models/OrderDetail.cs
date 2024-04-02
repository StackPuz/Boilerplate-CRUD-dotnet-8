using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models
{
    public partial class OrderDetail
    {
        [Key, Column(Order=1)]
        [Required]
        public int OrderId { get; set; }
        [Key, Column(Order=2)]
        [Required]
        public short No { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public short Qty { get; set; }
    }
}