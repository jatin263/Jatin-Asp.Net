using System.ComponentModel.DataAnnotations;

namespace Jatin.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage ="Name is Requied")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Username is Requied")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is Requied")]
        public string Password { get; set; }
        public string Profile_Path { get; set; }
    }

    public class UserView
    {
        [Required(ErrorMessage = "Name is Requied")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Username is Requied")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is Requied")]
        public string Password { get; set; }

        public IFormFile? Profile_Path { get; set; }
    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Username is Requied")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is Requied")]
        public string Password { get; set; }
    }
}
