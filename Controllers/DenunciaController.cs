using Microsoft.AspNetCore.Mvc;
using ProjetoFloresta.Data;
using ProjetoFloresta.Models;

namespace ProjetoFloresta.Controllers
{
    public class DenunciaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
