using Demo.DAL.Enums;
using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.ViewModels.Coupon;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.PL.Areas.Dashboard.Services
{
    public class CouponService
    {
        public bool ValidateCoupon(CreateCouponViewModel model, out DiscountType discountType, ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                model.Types = GetDiscountTypes();
                discountType = default;
                return false;
            }

            if (!Enum.TryParse<DiscountType>(model.SelectedType.ToString(), out discountType))
            {
                model.Types = GetDiscountTypes();
                return false;
            }

            if (discountType == DiscountType.Percentage && (model.Discount < 0 || model.Discount > 1))
            {
                modelState.AddModelError("Discount", "Value must be between 0.00 and 1.00");
                model.Types = GetDiscountTypes();
                return false;
            }

            if (model.EndDate.CompareTo(model.StartDate) <= 0)
            {
                modelState.AddModelError("EndDate", "End Date is same or earlier than Start Date");
                model.Types = GetDiscountTypes();
                return false;
            }

            return true;
        }
        public Coupon MapToCoupon(CreateCouponViewModel model, DiscountType discountType, bool isNew = false)
        {
            return new Coupon
            {
                Id = isNew ? Guid.NewGuid() : model.Id,
                Code = model.Code,
                Type = discountType,
                Discount = model.Discount,
                Status = model.Status,
                StartDate = model.StartDate.ToUniversalTime(),
                EndDate = model.EndDate.ToUniversalTime(),
                CreatedAt = isNew ? DateTime.UtcNow : model.CreatedAt
            };
        }
        
        public CreateCouponViewModel MapToCreateCouponVM(Coupon model)
        {
            return new CreateCouponViewModel()
            {
                Id = model.Id,
                Code = model.Code,
                Discount = model.Discount,
                Status = model.Status,
                StartDate = model.StartDate.ToUniversalTime(),
                EndDate = model.EndDate.ToUniversalTime(),
                CreatedAt = model.CreatedAt,
                Types = GetDiscountTypes(),
            };
        }
        public List<SelectListItem> GetDiscountTypes()
        {
            return new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Value = DiscountType.Percentage.ToString(),
                        Text = nameof(DiscountType.Percentage)
                    },
                    new SelectListItem
                    {
                        Value = DiscountType.FixedAmount.ToString(),
                        Text = nameof(DiscountType.FixedAmount)
                    }
                };
        }
    }
}
