using System.ComponentModel.DataAnnotations;

namespace AutoPartsManagementSystem.ViewModels
{
    public class CheckoutInput
    {
        public int? CustomerId { get; set; }

        [AllowedValues("Cash", "Card", "Transfer")]
        public string PaymentMethod { get; set; } = "Cash";
        
        [Range(0, 100)]
        public decimal DiscountPercent { get; set; }

        public ICollection<CheckoutItemInput> Items { get; set; } = new List<CheckoutItemInput>();
    }
}
