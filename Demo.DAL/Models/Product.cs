using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Demo.DAL.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }
        public bool InPublish { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; } = null!;
        public ICollection<ProductColor> ProductColors { get; set; } = null!;
        public ICollection<CartItem> CartItems { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = null!;

    }
}
