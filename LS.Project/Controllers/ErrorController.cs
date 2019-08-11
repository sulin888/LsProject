using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS.Project.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult InvalidResponse()
        {
            return View();
        }
        public ActionResult Other()
        {
            return View();
        }
        public ActionResult ServerInternal()
        {
            return View();
        }
        public ActionResult TimeOut()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult NotAuthorize()
        {
            return View();
        }
        public  ActionResult CustomerMsg()
        {
            ViewBag.CustomMsg = Request.QueryString["msg"];
            return View();
        }
    }
}