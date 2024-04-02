using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace App.ViewModels.Product.Index
{
    public class Product
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; }
        public string UserAccountName { get; set; }
    }
}

namespace App.ViewModels.Product.Create
{
    public class Product
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int BrandId { get; set; }
        [MaxLength(50)]
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

namespace App.ViewModels.Product.Detail
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; }
        public string UserAccountName { get; set; }
        public string Image { get; set; }
    }
}

namespace App.ViewModels.Product.Edit
{
    public class Product
    {
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
        public IFormFile ImageFile { get; set; }
    }

    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

namespace App.ViewModels.Product.Delete
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; }
        public string UserAccountName { get; set; }
        public string Image { get; set; }
    }
}
