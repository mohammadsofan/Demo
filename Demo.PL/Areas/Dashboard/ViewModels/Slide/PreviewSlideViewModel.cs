using System.Security.Policy;

namespace Demo.PL.Areas.Dashboard.ViewModels.Slide
{
    public class PreviewSlideViewModel
    {
        public string? Image { get; set; } = null!;
        public string? ImageBase64 { get; set; } = null!;
        public string SubTitle { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? SubLeft { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string? SubRight { get; set; } = null!;
        public string ButtonText { get; set; } = null!;
        public string Link { get; set; } = null!;
    }
}
