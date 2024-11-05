namespace Ecom.Data.Models.Models
{
    public class Orders
    {
        public int? id { get; set; }
        public string? prodName { get; set; }
        public int qty { get; set; }
        public decimal? totPrice { get; set; }
        public string customer { get; set; }
    }
}
