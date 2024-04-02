using System;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Brand.Index
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

namespace App.ViewModels.Brand.Create
{
    public class Brand
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}

namespace App.ViewModels.Brand.Detail
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BrandProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

namespace App.ViewModels.Brand.Edit
{
    public class Brand
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }

    public class BrandProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }
    }
}

namespace App.ViewModels.Brand.Delete
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BrandProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
