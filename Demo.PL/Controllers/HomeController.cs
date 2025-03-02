using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.PL.Areas.Dashboard.ViewModels.Slide;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Demo.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISlideRepository slideRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICardRepository cardRepository;
        private readonly IMapper mapper;

        public HomeController(ILogger<HomeController> logger,ISlideRepository slideRepository,ICategoryRepository categoryRepository,ICardRepository cardRepository,IMapper mapper)
        {
            this._logger = logger;
            this.slideRepository = slideRepository;
            this.categoryRepository = categoryRepository;
            this.cardRepository = cardRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            
            var slides = await slideRepository.GetAll();
            var categories = await categoryRepository.GetPopular();
            var cardsSectionA = await cardRepository.GetAllSectionA();
            var cardsSectionB = await cardRepository.GetAllSectionB();
            var homeVM = new HomeViewModel()
            {
                IntroSlides = mapper.Map<IEnumerable<PreviewSlideViewModel>>(slides),
                Categories = mapper.Map<IEnumerable<HomeCategoryViewModel>>(categories),
                CardSectionA = mapper.Map<IEnumerable<HomeCardViewModel>>(cardsSectionA),
                CardSectionB = mapper.Map<IEnumerable<HomeCardViewModel>>(cardsSectionB),
        };
            return View(homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
