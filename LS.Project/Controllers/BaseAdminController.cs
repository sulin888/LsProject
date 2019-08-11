using LS.Common;
using LS.Entitys;
using LS.Project.Helper;
using LS.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS.Project.Controllers
{
    public class BaseAdminController : Controller
    {

        protected SysUserModel UserInfo
        {
            get;set;
        }
       
        public List<SysMenuModel> CurentMenu { get; set; }

        public Dictionary<string, List<string>> CurentMenuPower { get; set; }
        public BaseAdminController()
        {
           
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var loginProvider = RequestHelper.GetSystemConfigByKey("LoginProvider");
            if(loginProvider== "Cookie")
            {
                var jsonuser= CookieHelper.GetCookieValue(SysConstant.SEESIONUSERKEY);
                if (!string.IsNullOrEmpty(jsonuser))
                {
                    UserInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<SysUserModel>(DESEncryptHelper.Decrypt(jsonuser));
                }
            }
            else
            {
                UserInfo= (SysUserModel)Session[SysConstant.SEESIONUSERKEY];
            }
            //if (UserInfo == null && !filterContext.ActionDescriptor.ActionName.Contains("Login"))
            //{
            //   // filterContext.Result = new RedirectResult("/Login");
            //    return;
            //}
            if (UserInfo == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.StatusCode = 401;//这个可以指定为其他的

                    //filterContext.Result = new JsonResult
                    //{
                    //    Data = "您长时间没有操作，请重新登录！",
                    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    //};
                    filterContext.HttpContext.Response.End();
                }
                else
                {
                    //重置漏油
                    filterContext.Result = RedirectToRoute("Default", new { Controller = "Error", Action = "TimeOut" });
                }
            }
            else
            {
                ViewBag.UserName = UserInfo.UserName;
                ViewBag.UserPhoto = UserInfo.UserPhoto;
                 
                if(CacheHelper.Get<List<SysMenuModel>>("CurentMenu_" + UserInfo.UserCode) != null)
                {
                    CurentMenu = CacheHelper.Get<List<SysMenuModel>>("CurentMenu_" + UserInfo.UserCode);
                    CurentMenuPower= CacheHelper.Get<Dictionary<string, List<string>>>("CurentPower_" + UserInfo.UserCode);
                }
                else
                {
                    GetUserMenu(UserInfo.UserCode);
                }
                base.OnActionExecuting(filterContext);
            }
        }

        #region 错误跳转
        /// <summary>
        /// 无效的响应
        /// </summary>
        /// <returns></returns>
        protected ActionResult ErrorInvalidResponse()
        {
            return RedirectToRoute("Default", new { Controller = "Error", Action = "InvalidResponse" });
        }
        /// <summary>
        /// 其它错误
        /// </summary>
        /// <returns></returns>
        protected ActionResult ErrorOther()
        {
            return RedirectToRoute("Default", new { Controller = "Error", Action = "Other" });
        }
        /// <summary>
        ///服务器内部
        /// </summary>
        /// <returns></returns>
        protected ActionResult ErrorServerInternal()
        {
            return RedirectToRoute("Default", new { Controller = "Error", Action = "ServerInternal" });
        }
        /// <summary>
        ///登陆超时
        /// </summary>
        /// <returns></returns>
        protected ActionResult ErrorTimeOut()
        {
            return RedirectToRoute("Default", new { Controller = "Error", Action = "TimeOut" });
        }
        /// <summary>
        ///404 没有找到
        /// </summary>
        /// <returns></returns>
        protected ActionResult ErrorNotFound()
        {
            return RedirectToRoute("Default", new { Controller = "Error", Action = "NotFound" });
        }
        /// <summary>
        ///没有权限
        /// </summary>
        /// <returns></returns>
        protected ActionResult ErrorNotAuthorize()
        {
            return  RedirectToRoute("Default", new { Controller = "Error", Action = "NotAuthorize" });
        }
        /// <summary>
        ///自定义消息
        /// </summary>
        /// <returns></returns>
        protected ActionResult ErrorCustomMsg(string strmsg)
        {
            return RedirectToRoute("Default", new { Controller = "Error", Action = "CustomerMsg", msg=strmsg });
        }
        #endregion

        #region 私有方法

        private void  GetUserMenu(string userCode)
        {
            IList<SysMenuEntity> menulist = ContainerBuilderHelper.Instance.GetUserPartMenu(userCode);
            menulist = GetDistinctMenu(menulist);
            List<SysMenuModel> lsetm = new List<SysMenuModel>();
            if (menulist != null)
            {
                var onemenulist = menulist.Where(w => w.Menulevel == 0);
                foreach (var item in onemenulist)
                {
                    SysMenuModel etm = new SysMenuModel();
                    etm.id = item.Id.Value;
                    etm.text = item.Menuname;
                    etm.attributes = new AttributesModel
                    {
                        iframe = "1",
                        datalink = item.Menuurl
                    };
                    etm.iconCls = item.Menuicon;
                    GetUserChildrenMenu(etm, menulist, item.Menucode);
                    lsetm.Add(etm);
                }
            }
            CurentMenu= lsetm;
            CacheHelper.Insert("CurentMenu_" + UserInfo.UserCode, lsetm);

        }
        private void GetUserChildrenMenu(SysMenuModel etmModel, IList<SysMenuEntity> listpartmenu, string MenuCode)
        {

            var children = listpartmenu.Where(w => w.Parentcode == MenuCode);
            if (children != null && children.Count() > 0)
            {
                etmModel.children = new List<SysMenuModel>();

                foreach (var item in children)
                {
                    SysMenuModel etm = new SysMenuModel();
                    etm.id = item.Id.Value;
                    etm.text = item.Menuname;
                    etm.iconCls = item.Menuicon;
                    etm.attributes = new AttributesModel
                    {
                        iframe = "1",
                        datalink = item.Menuurl

                    };
                    GetUserChildrenMenu(etm, listpartmenu, item.Menucode);
                    etmModel.children.Add(etm);
                }
            }
        }

        private List<SysMenuEntity> GetDistinctMenu(IList<SysMenuEntity> sysMenuList)
        {
            Dictionary<string, List<string>> dicPower = new Dictionary<string, List<string>>();
            List<SysMenuEntity> list = new List<SysMenuEntity>();
            foreach (var item in sysMenuList)
            {
                if (!dicPower.ContainsKey(item.Menucode))
                {
                    list.Add(item);
                    var powerls = dicPower.ContainsKey(item.Menucode)? dicPower[item.Menucode]: new List<string>();
                    if (!string.IsNullOrEmpty(item.Funcode))
                    {
                        string[] powers = item.Funcode.Split(',');
                        for (int i = 0; i < powers.Length; i++)
                        {
                            if (!powerls.Contains(powers[i]))
                            {
                                powerls.Add(powers[i]);
                            }
                        }
                        dicPower[item.Menucode] = powerls;
                    }
                   
                }
                else
                {
                    var powerls = dicPower.ContainsKey(item.Menucode) ? dicPower[item.Menucode] : new List<string>();
                    
                    if (!string.IsNullOrEmpty(item.Funcode))
                    {
                        string[] powers = item.Funcode.Split(',');
                        for (int i = 0; i < powers.Length; i++)
                        {
                            if (!powerls.Contains(powers[i]))
                            {
                                powerls.Add(powers[i]);
                            }
                        }
                        dicPower[item.Menucode] = powerls;
                    }

                }
            }
            CurentMenuPower = dicPower;
            CacheHelper.Insert("CurentPower_" + UserInfo.UserCode, dicPower);
            return list;
        }
        #endregion

        #region 权限验证
        public bool SysPower(string menuCode,string funCode,string userCode,bool isRedirect=true)
        {
            bool result = false;
            if ("ADMIN" == userCode)//管理员
            {
                result= true;
            }
            else if (!CurentMenuPower.ContainsKey(menuCode) || !CurentMenuPower[menuCode].Contains(funCode))
            {
                result = false;
            }
            if (isRedirect && !result)
            {
                Response.Redirect("/Error/NotAuthorize");
            }
            return result;
        }
        #endregion
    }
}