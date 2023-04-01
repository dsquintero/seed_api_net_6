using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string UserCode { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Password { get; set; }
    }
}
