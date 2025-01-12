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
        public string Name { get; set; } = null!;
        public double Discount { get; set; }
        public DateTime CraetedDate { get; set; }

        public ICollection<Order> Orders { get; set; } = null!;
    }
}
