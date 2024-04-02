using System;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.UserAccount.Index
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }
}

namespace App.ViewModels.UserAccount.Create
{
    public class UserAccount
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        public bool Active { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

namespace App.ViewModels.UserAccount.Detail
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }

    public class UserAccountUserRole
    {
        public string RoleName { get; set; }
    }
}

namespace App.ViewModels.UserAccount.Edit
{
    public class UserAccount
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        [Required]
        public bool Active { get; set; }
    }

    public class UserAccountUserRole
    {
        public int RoleId { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

namespace App.ViewModels.UserAccount.Delete
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
    }

    public class UserAccountUserRole
    {
        public string RoleName { get; set; }
    }
}
