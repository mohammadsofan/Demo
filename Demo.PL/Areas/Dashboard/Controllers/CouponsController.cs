using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Enums;
using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.Services;
using Demo.PL.Areas.Dashboard.ViewModels.Coupon;
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
        private readonly CouponService couponService;
        private readonly IMapper mapper;

        public CouponsController(ICouponRepository couponRepository,CouponService couponService,IMapper mapper)
        {
            this.couponRepository = couponRepository;
            this.couponService = couponService;
            this.mapper = mapper;
        }
        [HttpGet("/Dashboard/Coupons")]
        public IActionResult RedirectToIndex()
        {
            return RedirectToAction(nameof(Index));
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
                Types = couponService.GetDiscountTypes(),
            };
            return View(coupon);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateCouponViewModel model)
        {
            try
            {
                if (!couponService.ValidateCoupon(model, out var discountType,ModelState))
                {
                    return View(model);
                }

                var coupon = couponService.MapToCoupon(model, discountType, isNew: true);

                var result = await couponRepository.Create(coupon);

                if (result == 1)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("Code", "This code is already used");
                model.Types = couponService.GetDiscountTypes();
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }

                var coupon = await couponRepository.Get(id.Value);
                if (coupon is null)
                {
                    return NotFound();
                }
                ViewBag.CouponCode=coupon.Code;
                return View(id);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(Guid? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }

                var coupon = await couponRepository.Get(id.Value);
                if (coupon is null)
                {
                    return NotFound();
                }
                await couponRepository.Delete(coupon);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if(id is null)
            {
                return BadRequest();
            }
            var coupon = await couponRepository.Get(id.Value);
            if(coupon is null)
            {
                return NotFound();
            }
            var couponVM = couponService.MapToCreateCouponVM(coupon);
            return View(couponVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateCouponViewModel model)
        {
            try
            {
                if (!couponService.ValidateCoupon(model, out var discountType,ModelState))
                {
                    return View(model);
                }

                var coupon = couponService.MapToCoupon(model, discountType);

                var result = await couponRepository.Update(coupon);

                if (result == 1)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("Code", "This code is already used");
                model.Types = couponService.GetDiscountTypes();
                return View(model);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}
