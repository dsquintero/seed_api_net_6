using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class RefreshRequest
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string RefreshToken { get; set; }
    }
}
