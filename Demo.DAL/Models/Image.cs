using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid ProductColorId { get; set; }
        public ProductColor ProductColor { get; set; } = null!;
    }
}
