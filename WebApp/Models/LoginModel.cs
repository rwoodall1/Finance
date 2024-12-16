using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class User
    {
        [Required(ErrorMessage ="User Name Missing")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Missing")]
        public string Password { get; set; }
    
    }
}
