// Using statements
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Portafolio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
#pragma warning disable CS8618
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    // Add a private variable of type MyContext (or whatever you named your context file)
    private MyContext _context;
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it   
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

/* Metodos Get */


    [HttpGet("")]
    public IActionResult Index()
    {        


        return View("Index");
    }




    [HttpGet]
    [Route("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return View("Index");
    }



    [SessionCheck]
    [HttpGet]
    [Route("posts")]
    public IActionResult Posts()
    {
        List<Post> ListaPosts = _context.Posts
                                        .Include(po => po.Creador)
                                        .OrderByDescending(po => po.Created_at)
                                        .ToList();
        return View("Posts", ListaPosts);
    }

    [SessionCheck]
    [HttpGet]
    [Route("post/new")]
    public IActionResult FormularioPost(User nuevoPost)
    {

        return View("FormularioPost");
    }
    [SessionCheck]
    [HttpGet]
    [Route("posts/{id_post}")]
    public IActionResult Detalle(int id_post)
    {

        Post DetallePost = _context.Posts
                                    .Include(po => po.Creador)
                                    .FirstOrDefault(a => a.PostId == id_post);
        if (DetallePost == null)
        {
            return RedirectToAction("Posts");
        }
        return View(DetallePost);
    }


    [SessionCheck]
    [HttpGet]
    [Route("posts/edit/{id_post}")]
    public IActionResult FormularioEditarPost(int id_post)
    {
        Post? post = _context.Posts.FirstOrDefault(po => po.PostId == id_post);
        return View("FormularioEditarPost", post);
    }


    // Metodos POST





    [HttpPost]
    [Route("procesa/login")]
    public IActionResult ProcesaLogin(Login login)
    {
        if (ModelState.IsValid)
        {
            User? usuario = _context.Users.FirstOrDefault(u => u.Email == login.Email);

            if (usuario != null)
            {
                //DENCRIPTAMOS EL PASWORD PARA COMPARAR CREDENCIALES
                PasswordHasher<Login> Hasher = new PasswordHasher<Login>();
                var result = Hasher.VerifyHashedPassword(login, usuario.Password, login.Password);
                if (result != 0)
                {
                    HttpContext.Session.SetString("Nombre", usuario.Name);
                    HttpContext.Session.SetString("Email", usuario.Email);
                    HttpContext.Session.SetInt32("Id", usuario.UserId);
                    return RedirectToAction("Posts", "Home");
                    
                }
            }
            ModelState.AddModelError("Password", "El correo o el passwrod es incorrecto");
            return View("Index");

        }
        return View("Index");
    }


     /* Metodos relacionados con los Post de la aplicacion */

    [HttpPost]
    [Route("post/new/procesa")]
    public IActionResult AgregarPost(Post post)
    {
        string? email = HttpContext.Session.GetString("Email");
        if (email == null)
        {
            return View("Login");
        }
        if (ModelState.IsValid)
        {
            /* INSERT INTO pelicula(titulo, genero, anio, descripcion, director_id)
               VALUES(titulo, genero, anio, descripcion, director_id); */
            post.UserId = (int)HttpContext.Session.GetInt32("Id");


            _context.Posts.Add(post);
            _context.SaveChanges();
            return RedirectToAction("Posts");
        }
        return View("FormularioPost");
    }

    [HttpPost]
    [Route("eliminar/post{id_post}")]
    public IActionResult EliminarPost(int id_post)
    {
        Post? post = _context.Posts.FirstOrDefault(po => po.PostId == id_post);
        _context.Posts.Remove(post);
        _context.SaveChanges();

        return RedirectToAction("Posts");
    }

    [HttpPost]
    [Route("actualiza/post/{id_post}")]
    public IActionResult ActualizaPost(Post post, int id_post)
    {
        Post? postAntiguo = _context.Posts.FirstOrDefault(po => po.PostId == id_post);

        if (ModelState.IsValid)
        {
            postAntiguo.Image = post.Image;
            postAntiguo.Title = post.Title;
            postAntiguo.Medium = post.Medium;
            postAntiguo.Sale = post.Sale;
            //postAntiguo.Fecha_Actualizacion = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Posts");
        }
        return View("FormularioEditarPost", postAntiguo);

    }


    [HttpPost]
    [Route("actualiza/like/{id_post}")]
    public IActionResult ActualizarLike(int id_post)
    {
        Post? postAntiguo = _context.Posts.FirstOrDefault(po => po.PostId == id_post);

        if (postAntiguo != null)
        {
            postAntiguo.Like += 1;
            _context.SaveChanges();
        }

        return RedirectToAction("Posts");
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    // Name this anything you want with the word "Attribute" at the end




    /* Session Check */
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            string? email = context.HttpContext.Session.GetString("Email");

            
            if (email == null)
            {
                
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }



}

