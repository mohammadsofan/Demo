using Demo.PL.Areas.Dashboard.ViewModels.Image;

namespace Demo.PL.Areas.Dashboard.ViewModels.ProductColor
{
    public class EditProductColorViewModel
    {
        public Guid Id { get; set; }
        public string HexCode { get; set; } = null!;
        public Guid ProductId { get; set; }
        public List<ImageViewModel>? Images { get; set; } = null!;
        public List<IFormFile>? NewImages { get; set; } = null!;
    }
}
