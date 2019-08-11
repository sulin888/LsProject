using LS.Cores;
using LS.Entitys;
using LS.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS.Project.Controllers
{
    public class AdminController : BaseAdminController
    {
        ISysMenu ISysMenuService;

        public AdminController(ISysMenu repositoryMenu)
        {
            ISysMenuService = repositoryMenu;
        }

        /// <summary>
        /// 管理员首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["ParentMenu"] = CurentMenu;
            return View();
        }
        public ActionResult Welcome()
        {
            return View();
        }

        #region 公有方法

        #endregion


    }
}