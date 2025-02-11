namespace Demo.PL.Areas.Dashboard.ViewModels.Card
{
    public class EditCardViewModel:CardViewModel
    {
        public IFormFile? NewImage { get; set; } = null!;
    }
}
