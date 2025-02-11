using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class ProductColor
    {
        public Guid Id { get; set; }
        public string HexCode { get; set; } = null!;
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual IEnumerable<Image> Images { get; set; } = null!;
    }
}
