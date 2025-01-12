﻿using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Areas.Dashboard.ViewModels
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
