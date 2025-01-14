using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Enums;
using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.PL.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles = "Admin")]
    public class CouponsController : Controller
    {
        private readonly ICouponRepository couponRepository;
        private readonly IMapper mapper;

        public CouponsController(ICouponRepository couponRepository,IMapper mapper)
        {
            this.couponRepository = couponRepository;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var coupons = await couponRepository.GetAll();
                var couponVM = mapper.Map<IEnumerable<CouponViewModel>>(coupons); 
                return View(couponVM);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IActionResult Create()
        {
            var coupon = new CreateCouponViewModel()
            {
                Types = GetDiscountTypes(),
            };
            return View(coupon);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCouponViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    model.Types = GetDiscountTypes();
                    return View(model);
                }

                if (!Enum.TryParse<DiscountType>(model.SelectedType.ToString(), out var discountType))
                {
                    model.Types = GetDiscountTypes();
                    return View(model);
                }

                if (discountType == DiscountType.Percentage && (model.Discount < 0 || model.Discount > 1))
                {
                    ModelState.AddModelError("Discount", "Value must be between 0.00 and 1.00");
                    model.Types = GetDiscountTypes();
                    return View(model);
                }
                if (model.EndDate.CompareTo(model.StartDate) <= 0)
                {
                    ModelState.AddModelError("EndDate", "End Date is same or earlier than Start Date");
                    model.Types = GetDiscountTypes();
                    return View(model);
                }

                var coupon = new Coupon
                {
                    Id = Guid.NewGuid(),
                    Code = model.Code,
                    Type = discountType,
                    Discount = model.Discount,
                    Status=model.Status,
                    StartDate=model.StartDate.ToUniversalTime(),
                    EndDate=model.EndDate.ToUniversalTime(),
                    CreatedAt = DateTime.UtcNow
                };

                var result = await couponRepository.Create(coupon);

                if (result == 1)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("Code", "This code is already used");
                model.Types = GetDiscountTypes();
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private List<SelectListItem> GetDiscountTypes()
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
