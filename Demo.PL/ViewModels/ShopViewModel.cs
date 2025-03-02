using Demo.DAL.Models;

namespace Demo.PL.ViewModels
{
    public class ShopViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = null!;
    }
}
