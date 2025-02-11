namespace Demo.PL.Areas.Dashboard.ViewModels.Slide
{
    public class SlideViewModel
    {
        public Guid Id { get; set; }
        public string? Image { get; set; } = null!;
        public string? SubTitle { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? SubLeft { get; set; } = null!;
        public string? Price { get; set; } = null!;
        public string? SubRight { get; set; } = null!;
        public string ButtonText { get; set; } = null!;
        public string Link { get; set; } = null!;
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
