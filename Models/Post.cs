#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Portafolio.Models;
public class Post
{
    [Key]
    [Required]
    public int PostId { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Oops el minimo es de 2 caracteres")]
    public string Image { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Oops el minimo es de 2 caracteres")]
    public string Title { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Oops el minimo es de 2 caracteres")]
    public string Description { get; set; }



    public DateTime Created_at { get; set; } = DateTime.Now;
    public DateTime Updated_at { get; set; } = DateTime.Now;


    //LLAVE FORANEA
    public int UserId { get; set; }
    public User? Creador { get; set; }

}