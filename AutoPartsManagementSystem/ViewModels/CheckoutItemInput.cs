using System.ComponentModel.DataAnnotations;

namespace AutoPartsManagementSystem.ViewModels
{
    public class CheckoutItemInput
    {
        public int ProductId { get; set; }

        [Range(1, 1000)]
        public int Quantity { get; set; }
    }
}