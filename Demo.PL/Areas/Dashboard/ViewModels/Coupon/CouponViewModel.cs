using Demo.DAL.Enums;
using Demo.DAL.Models;

namespace Demo.PL.Areas.Dashboard.ViewModels.Coupon
{
    public class CouponViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public DiscountType Type { get; set; }
        public double Discount { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
