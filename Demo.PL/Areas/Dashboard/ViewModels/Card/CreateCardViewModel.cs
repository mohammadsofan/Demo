namespace Demo.PL.Areas.Dashboard.ViewModels.Card
{
    public class CreateCardViewModel:CardViewModel
    {
        public IFormFile NewImage { get; set; } = null!;
    }
}
