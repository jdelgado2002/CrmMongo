using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CRMMongo.Properties;
using MongoDB.Driver;

namespace CRMMongo.Controllers {
	[Authorize]
	public class HomeController : Controller {
		public MongoDatabase Database;
		public HomeController() {
			var client = new MongoClient(Settings.Default.mongoDbConnectionString);
			var server = client.GetServer();
			Database = server.GetDatabase(Settings.Default.simlesDatabase);
		}

		public ActionResult Index() {
			//return Json(Database.Server.BuildInfo, JsonRequestBehavior.AllowGet);
			return View();
		}
	}
}
