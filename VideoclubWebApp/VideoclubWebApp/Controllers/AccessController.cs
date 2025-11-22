using Microsoft.AspNetCore.Mvc;

namespace VideoclubWebApp.Controllers
{
    public class AccessController : Controller
    {
        public IActionResult Denied()
        {
            return View();
        }
    }
}