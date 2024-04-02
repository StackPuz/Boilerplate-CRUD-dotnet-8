using System;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.OrderDetail.Create
{
    public class OrderDetail
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public short No { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public short Qty { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

namespace App.ViewModels.OrderDetail.Edit
{
    public class OrderDetail
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public short No { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public short Qty { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

namespace App.ViewModels.OrderDetail.Delete
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public short No { get; set; }
        public string ProductName { get; set; }
        public short Qty { get; set; }
    }
}
