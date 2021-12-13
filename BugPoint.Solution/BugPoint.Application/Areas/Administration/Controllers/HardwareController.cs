using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugPoint.Application.Areas.Administration.Controllers
{

    [Area("Administration")]
    public class HardwareController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
