using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public  class Order
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = null!;
        public double TotalPrice { get; set; }
        public double FinalPrice { get; set; }
        public DateTime OrderDate { get; set; }

        public string UserId { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;

        public Guid? CoponId { get; set; }
        public virtual Coupon Copon { get; set; } = null!;

        public virtual ICollection<OrderItem> OrderItems { get; set; } = null!;
    }
}   
