using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Areas.Dashboard.ViewModels
{
    public class ProductColorViewModel
    {
        public Guid Id { get; set; }
        public string HexCode { get; set; } = null!;
        public Guid ProductId { get; set; }
        [Required(ErrorMessage = "Please upload at least one image.")]

        public List<IFormFile> Images { get; set; } = null!;

    }
}
