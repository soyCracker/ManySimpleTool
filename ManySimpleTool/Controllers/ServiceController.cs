using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ManySimpleTool.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult QRCodeTool()
        {
            return View();
        }
    }
}
