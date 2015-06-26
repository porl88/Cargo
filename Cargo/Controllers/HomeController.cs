namespace Cargo.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;

	public class HomeController : Controller
    {
        // GET: /home
        public ViewResult Index()
        {
			return View();
        }
    }
}
