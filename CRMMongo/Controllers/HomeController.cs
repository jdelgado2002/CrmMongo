using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CRMMongo.Properties;
using MongoDB.Driver;

namespace CRMMongo.Controllers {
	[Authorize]
	public class HomeController : Controller {
		
		public ActionResult Index() {
			return View();
		}
	}
}
