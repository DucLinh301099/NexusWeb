using System.ComponentModel.DataAnnotations;

namespace NexusWeb.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string? UserName  { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [Compare("Password",ErrorMessage = "Password don't match")]
        public string? ConfirmPassword { get; set; }


    }
}
