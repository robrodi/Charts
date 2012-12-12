using AdminData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace AdminData.Controllers
{
    public class HomeController : Controller
    {
        const string dataPath = "~/App_Data/data.txt";
        //
        // GET: /Home/
//        [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }


        [OutputCache(Duration = 3600, VaryByParam = "none")]
        public ContentResult HopperTSV()
        {
            //var reader= new HopperCountReader(HostingEnvironment.MapPath(dataPath));
            var content = HopperCountReader.JunkTSV(5);
            return Content(content);
        }
    }
}
