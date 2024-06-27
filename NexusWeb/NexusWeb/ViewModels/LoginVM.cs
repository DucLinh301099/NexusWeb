using System.ComponentModel.DataAnnotations;

namespace NexusWeb.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage ="User Name is Required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
