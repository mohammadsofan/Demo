using Demo.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Coupon
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public DiscountType Type { get; set; }
        public double Discount { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = null!;
    }
}
