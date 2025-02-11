using Demo.DAL.Models;
using Demo.PL.Areas.Dashboard.ViewModels.Slide;

namespace Demo.PL.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<PreviewSlideViewModel> IntroSlides { get; set; } = null!;
        public IEnumerable<HomeCategoryViewModel> Categories { get; set; } = null!;
        public IEnumerable<HomeCardViewModel> CardSectionA { get; set; } = null!;
        public IEnumerable<HomeCardViewModel> CardSectionB { get; set; } = null!;
    }
}
