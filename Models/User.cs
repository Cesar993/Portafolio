#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Portafolio.Models;
public class User
{
    [Key]
    [Required]
    public int UserId { get; set; }

    [EmailAddress(ErrorMessage = "por favor proporciona un correo valido")]
    [MinLength(2, ErrorMessage = "Oops el minimo es de 2 caracteres")]
    [Required]
    public string Email { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Oops el minimo es de 8 caracteres")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public DateTime Created_at {get;set;} = DateTime.Now;
    public DateTime Updated_at {get;set;} = DateTime.Now;


    [NotMapped]
    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    [Display(Name = "Pasword confirmado")]
    public string PasswordConfirm { get; set; }

    public List<Post> ListaPosts {get;set;}= new List<Post>();


}