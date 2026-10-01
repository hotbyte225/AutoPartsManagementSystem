using AutoPartsManagementSystem.Models;


namespace AutoPartsManagementSystem.ViewModels
{
    public class SaleViewModel
    {
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Customer> Customers { get; set; } = new List<Customer>();

    }
}
