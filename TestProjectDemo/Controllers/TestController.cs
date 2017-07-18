using AttributeRouting;
using AttributeRouting.Web.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestProjectDemo.Controllers
{
   
    public class TestController : Controller
    {

        [ActionName("test-action")]
       
        public ActionResult TestAction()
        {
            return View("TestAction");
        }
        [HttpPost]
        [ActionName("test-action")]
        public ActionResult TestAction(HttpPostedFileBase fileName)
        {

            var path = Path.GetFullPath(fileName.FileName);
            return View("TestAction");
        }
    }
}
