using Microsoft.AspNetCore.Mvc;
using Firebase.Auth;
using MVC.Models;
using System.Diagnostics;
using Firebase.Storage;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public async Task<ActionResult> SubirArchivo(IFormFile archivo)
        {
            Stream archivoASubir = archivo.OpenReadStream();
            string email = "nathaly.matute@catolica.edu.sv";
            string clave = "nathaly12";
            string ruta = "gs://p11mvc-f114c.appspot.com";
            string api_key = "AIzaSyBGl5jJhQ2C-YknkUCZ40ChlGe_5kJ5Pzw";


            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFireBase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFireBase.FirebaseToken;

            var tareaCargarArchivo = new FirebaseStorage( ruta,
                                                                new FirebaseStorageOptions
                                                                {
                                                                    AuthTokenAsyncFactory = () => Task.FromResult(tokenUser), 
                                                                    ThrowOnCancel = true
                                                                }).Child("Archivos")
                                                                .Child(archivo.FileName)
                                                                .PutAsync(archivoASubir, cancellation.Token);
            var urlArchivoCargado = await tareaCargarArchivo;

            return RedirectToAction("VerImagen");






          
        }       
   
    }
}