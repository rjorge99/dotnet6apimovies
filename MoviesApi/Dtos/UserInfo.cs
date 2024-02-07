using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class UserInfo
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
