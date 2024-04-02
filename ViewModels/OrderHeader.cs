using System;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.OrderHeader.Index
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
    }
}

namespace App.ViewModels.OrderHeader.Create
{
    public class OrderHeader
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

namespace App.ViewModels.OrderHeader.Detail
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class OrderHeaderOrderDetail
    {
        public short No { get; set; }
        public string ProductName { get; set; }
        public short Qty { get; set; }
    }
}

namespace App.ViewModels.OrderHeader.Edit
{
    public class OrderHeader
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
    }

    public class OrderHeaderOrderDetail
    {
        public short No { get; set; }
        public string ProductName { get; set; }
        public short Qty { get; set; }
        public int OrderId { get; set; }
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

namespace App.ViewModels.OrderHeader.Delete
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class OrderHeaderOrderDetail
    {
        public short No { get; set; }
        public string ProductName { get; set; }
        public short Qty { get; set; }
    }
}
