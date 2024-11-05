using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Data.Models.Models
{
    public class Orderproducts
    {
        public int? id { get; set; }
        public string prodName { get; set; }
        public int stock { get; set; }
        public decimal price { get; set; }
        public int prodId { get; set; }
    }
}
