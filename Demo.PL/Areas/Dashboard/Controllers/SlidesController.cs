using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.ViewModels.Slide;
using Demo.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Demo.PL.Areas.Dashboard.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Dashboard")]
    public class SlidesController : Controller
    {
        private readonly ISlideRepository slideRepository;
        private readonly IMapper mapper;

        public SlidesController(ISlideRepository slideRepository,IMapper mapper)
        {
            this.slideRepository = slideRepository;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var slides = await slideRepository.GetAll();
                var slidesVm =mapper.Map<IEnumerable<SlideViewModel>>(slides);
                return View(slidesVm);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSlideViewModel model,string action)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (action == "Preview")
                    {
                        var previewSlideVM = mapper.Map<PreviewSlideViewModel>(model);
                        previewSlideVM.ImageBase64 = await FileHelper.ConvertFileToBase64Async(model.NewImage);

                        HttpContext.Session.SetString("previewSlide", JsonConvert.SerializeObject(previewSlideVM));
                        return RedirectToAction(nameof(Preview));
                    }
                    var slide = mapper.Map<Slide>(model);
                    slide.Id = Guid.NewGuid();
                    slide.CreatedAt = DateTime.UtcNow;
                    slide.UpdatedAt = DateTime.UtcNow;
                    slide.Image = await FileHelper.UploadImage(model.NewImage, "images");
                    await slideRepository.Create(slide);
                    return RedirectToAction(nameof(Index));
                }
                else { 
                     return View(model);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public  async Task<IActionResult> Preview(Guid? id)
        {
            try
            {
                PreviewSlideViewModel previewSlideVm;
                //in case preview is coming from ShowAll(index) page
                if (id is not null)
                {
                     var slide = await slideRepository.Get(id.Value);
                     previewSlideVm = mapper.Map<PreviewSlideViewModel>(slide);
                }
                //in case preview is coming from Create|Edit page
                else
                {
                     previewSlideVm = JsonConvert.DeserializeObject<PreviewSlideViewModel>(HttpContext.Session.GetString("previewSlide"));
                }
                return View(previewSlideVm);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                if (id is null)
                    return Json(new { success = false, message = "Invalid Id" });
                var slide = await slideRepository.Get(id.Value);
                if (slide is null)
                {
                    return Json(new { success = false, message = "Invalid Slide object" });
                }
                FileHelper.DeleteImage("images", slide.Image);
                await slideRepository.Delete(slide);

                return Json(new { success = true, message = "Slide deleted successfully" });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the slide. Please try again later."+ex.Message });
            }

        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }
                var slide = await slideRepository.Get(id.Value);
                if (slide is null)
                {
                    return NotFound();
                }
                var slideVm =mapper.Map<EditSlideViewModel>(slide);
                return View(slideVm);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditSlideViewModel model,string action)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (action == "Preview")
                    {
                        var previewSlideVM = mapper.Map<PreviewSlideViewModel>(model);
                        if (model.NewImage is not null)
                        {
                            previewSlideVM.ImageBase64 = await FileHelper.ConvertFileToBase64Async(model.NewImage);
                        }
                        else
                        {
                            previewSlideVM.Image = model.Image;
                        }
                        HttpContext.Session.SetString("previewSlide", JsonConvert.SerializeObject(previewSlideVM));
                        return RedirectToAction(nameof(Preview));
                    }
                    if (model.NewImage is not null)
                    {
                        FileHelper.DeleteImage("images", model.Image??"");
                        model.Image = await FileHelper.UploadImage(model.NewImage, "images");
                    }
                    model.UpdatedAt=DateTime.UtcNow;
                    var slide = mapper.Map<Slide>(model);
                    await slideRepository.Update(slide);
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
