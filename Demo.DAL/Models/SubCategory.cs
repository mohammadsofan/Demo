﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class SubCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Product> Products { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

    }
}
