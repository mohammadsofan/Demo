using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Enums;
using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.ViewModels.Card;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Areas.Dashboard.Controllers
{
    [Authorize(Roles ="Admin")]
    [Area("Dashboard")]
    public class CardsController : Controller
    {
        private readonly ICardRepository cardRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CardsController> logger;

        public CardsController(ICardRepository cardRepository,IMapper mapper,ILogger<CardsController> logger) {
            this.cardRepository = cardRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                logger.Log(LogLevel.Information,"the admin {adminName} enterd the cards index page", User.Identity.Name);
                var cards = await cardRepository.GetAll();
                var cardsVM = mapper.Map<IEnumerable<CardViewModel>>(cards);
                return View(cardsVM);
            }
            catch(Exception ex)
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
        public async Task<IActionResult> Create(CreateCardViewModel model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    //check cards count
                    int maxCards = model.Type == CardType.SectionA ? 3 : 2;
                    var count = model.Type == CardType.SectionA
                        ? await cardRepository.GetSectionACount()
                        : await cardRepository.GetSectionBCount();

                    if (count >= maxCards)
                    {
                        ModelState.AddModelError(string.Empty, $"You can only add up to {maxCards} cards in {model.Type}. Please remove a card before adding a new one.");
                        return View(model);
                    }
                    //check if the order is valid
                    if(model.Type== CardType.SectionB&&(model.Order <0 || model.Order >2))
                    {
                        ModelState.AddModelError("Order", $"The order number '{model.Order}' is invalid.");
                        return View(model);
                    }
                    else if(model.Type == CardType.SectionA && (model.Order < 0 || model.Order > 3)){
                        ModelState.AddModelError("Order", $"The order number '{model.Order}' is invalid.");
                        return View(model);

                    }
                    //check if the order is already in use

                    var isExistedOrder = model.Type == CardType.SectionA
                        ? await cardRepository.IsExistedOrderSectionA(model.Order)
                        : await cardRepository.IsExistedOrderSectionB(model.Order);

                    if (isExistedOrder)
                    {
                        ModelState.AddModelError("Order", $"The order number '{model.Order}' already in use. Please choose a different order or update the existing one.");
                        return View(model);
                    }
                    var card = mapper.Map<Card>(model);
                    card.Id = Guid.NewGuid();
                    card.CreatedAt=DateTime.UtcNow;
                    card.UpdatedAt=DateTime.UtcNow;
                    card.Image = await FileHelper.UploadImage(model.NewImage, "images");
                    await cardRepository.Create(card);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest();
                }
                var card = await cardRepository.Get(id.Value);
                if (card is null)
                {
                    return NotFound();
                }
                var cardVm = mapper.Map<EditCardViewModel>(card);
                return View(cardVm);
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditCardViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //check if the order is valid
                    if (model.Type == CardType.SectionB && (model.Order < 0 || model.Order > 2))
                    {
                        ModelState.AddModelError("Order", $"The order number '{model.Order}' is invalid.");
                        return View(model);
                    }
                    else if (model.Type == CardType.SectionA && (model.Order < 0 || model.Order > 3))
                    {
                        ModelState.AddModelError("Order", $"The order number '{model.Order}' is invalid.");
                        return View(model);

                    }
                    var card = mapper.Map<Card>(model);
                    if(model.NewImage is not null)
                    {
                        FileHelper.DeleteImage("images", model.Image!);
                        card.Image = await FileHelper.UploadImage(model.NewImage,"images");
                    }
                    else
                    {
                        card.Image =model.Image!;
                    }
                    card.UpdatedAt = DateTime.UtcNow;
                    await cardRepository.Update(card);
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    return View(model);
                }
            }catch(Exception ex)
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
                var card = await cardRepository.Get(id.Value);
                if (card is null)
                {
                    return Json(new { success = false, message = "Invalid card object" });
                }
                FileHelper.DeleteImage("images", card.Image);
                await cardRepository.Delete(card);

                return Json(new { success = true, message = "Card deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while deleting the card. Please try again later."+ex.Message });
            }

        }

        public async Task<IActionResult> Preview()
        {
            var cards= await cardRepository.GetAll();
            var cardsVM = mapper.Map<IEnumerable<PreviewCardViewModel>>(cards);
            return View(cardsVM);
        }

    }
}
