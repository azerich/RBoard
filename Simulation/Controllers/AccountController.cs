using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login() => View();
    }
}
