using System.Collections.Generic;

namespace BookNest.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; } = new List<ShoppingCart>();
        public double OrderTotal { get; set; } = 0;
    }
}
