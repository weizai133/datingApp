using System.ComponentModel.DataAnnotations;

namespace backend.Dtos
{
    public class UserForLoginDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        [StringLength(8, MinimumLength=4, ErrorMessage="Password must be within 4 to 8 character")]
        public string password { get; set; }
    }
}