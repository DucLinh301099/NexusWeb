using NexusWeb.Models;
namespace NexusWeb.ViewModels
{
    public class InfoUserVM
    {
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Phone { get; set; }

        public int? UserId { get; set; }

        public string? Image { get; set; }

        public string? Note { get; set; }

        public virtual User? User { get; set; }
        public IFormFile? formFile { get; set; }
    }
}
