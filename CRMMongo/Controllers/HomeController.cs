using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CRMMongo.MongoContext;
using CRMMongo.Properties;
using MongoDB.Driver;

namespace CRMMongo.Controllers {
	[Authorize]
	public class HomeController : Controller {
		private IMongoContext Context { get; set; }
		public HomeController() {
			Context = new MongoContext.MongoContext();
		}

		public ActionResult Index() {
			//return Json(Context.Database.Settings, JsonRequestBehavior.AllowGet);
			return View();
		}
	}
}
