using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class UserRegisterDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be at least 4 character and maximum 8 character")]
        public string password { get; set; }
    }
}