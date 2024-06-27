using NexusWeb.Models;
namespace NexusWeb.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }

        public string? Name { get; set; } 

        public decimal Price { get; set; }

        public string? Description { get; set; } 

        public string? Detail { get; set; } 

        public int DistributorId { get; set; }

        public int CategoryId { get; set; }

        public IFormFile? ImageData { get; set; }

        public IFormFile[]? LImageData { get; set; }




    }
}
