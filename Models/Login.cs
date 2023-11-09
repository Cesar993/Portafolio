
#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace Portafolio.Models;
public class Login
{
    
    [EmailAddress(ErrorMessage = "por favor proporciona un correo valido")]
    [Required]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}