using LS.Common;
using LS.Cores;
using LS.Entitys;
using LS.Project.Controllers;
using LS.Project.Filters;
using LS.Project.Helper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS.Project.Areas.System.Controllers
{
    public class MenuController : BaseAdminController
    {
       
        #region MenuController
        ISysFunction ISysFunctionService;
        ISysMenu ISysMenuService;
        public MenuController(ISysFunction repositoryFun, ISysMenu repositoryMenu)
        {
            ISysFunctionService = repositoryFun;
            ISysMenuService = repositoryMenu;
        }
        #endregion
        // GET: SysSetting/SysMenu
        public ActionResult Index()
        {
            SysPower(SysMenuConstant.MENU_MANAGE, SysMenuConstant.FUN_SELECT, UserInfo.UserCode);
            ViewBag.FunList = JsonConvert.SerializeObject(ISysFunctionService.Query(string.Empty));
            return View();
        }
        public ActionResult GenerateCode()
        {
            return View();
        }
        public  ActionResult MsSql()
        {
            return View();
        }
       

        #region 页面
        /// <summary>
        /// 创建操作功能
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateFunction()
        {
            SysPower(SysMenuConstant.MENU_MANAGE, SysMenuConstant.FUN_ADD, UserInfo.UserCode);
            return View("EditFunction");
        }
        /// <summary>
        ///创建菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateMenu()
        {
            SysPower(SysMenuConstant.MENU_MANAGE, SysMenuConstant.FUN_ADD, UserInfo.UserCode);
            var model = new SysMenuEntity();
            ViewBag.FunList = ISysFunctionService.Query(string.Empty);
            model.Isvisible = false;
            model.Isenable = true;
            #region 增加是自动获取菜单代码和序列号
            string strwhere = string.Empty;
          
            Hashtable hs = ISysMenuService.GetMaxPartMenu(RequestHelper.FilterParam(Request.QueryString["code"]));
            string parentCode = "";
            int menseq = 0;
            if (hs.Count > 0)
            {
                parentCode = hs["MenuCode"].ToString();
                menseq = Convert.ToInt32(hs["MenuSeq"]);
            }
            string code = RequestHelper.GetQueryValueOrNull<string>("code");
            string currcode = "";
            if (parentCode == "")
            {
                if (string.IsNullOrEmpty(code))//表示顶级菜单
                {
                    currcode = "101";
                }
                else
                {
                    currcode = Request.QueryString["code"].ToString() + "001";
                }
                menseq = 1;
            }
            else
            {

                if (string.IsNullOrEmpty(code))//表示顶级菜单
                {
                    currcode = Convert.ToString(Convert.ToInt32(parentCode) + 1);
                    menseq = 1;
                }
                else
                {
                    currcode = code + Convert.ToString(Convert.ToInt32(parentCode.Substring(parentCode.Length - 3)) + 1).PadLeft(3, '0');
                    menseq++;
                }
            }
            model.Menucode = currcode;
            model.Menuseq = menseq;
            model.Parentcode = code;
            #endregion
            return View("EditMenu", model);
        }
        /// <summary>
        ///编辑菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult EditMenu(int id)
        {
            SysPower(SysMenuConstant.MENU_MANAGE, SysMenuConstant.FUN_UPDATE, UserInfo.UserCode);
            var model = ISysMenuService.GetById(id);
            if (model == null)
            {    
                return ErrorCustomMsg(ResponseHelper.NONEXISTENT);
            }
            ViewBag.FunList = ISysFunctionService.Query(string.Empty);
            ViewBag.MenuFunList = ISysMenuService.GetMenuFunction(" and menucode ='"+ model.Menucode+ "'");
            return View(model);
        }
        #endregion

        #region  公共方法
        /// <summary>
        /// 保存功能
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveFunction(SysFunctionEntity model)
        {
            var fnamelen = StringHelper.GetStrLength(model.Funname);
            if (fnamelen == 0 || fnamelen > 10)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "名称不合法" });
            }
            if (string.IsNullOrWhiteSpace( model.Funcode))
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "代码不能为空" });
            }
            int result= ISysFunctionService.Insert(model);
            if (result > 0)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "保存成功" });
            }
            else
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "保存失败" });
            }
        }

        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveMenu(SysMenuEntity model)
        {
            var menuname = StringHelper.GetStrLength(model.Menuname);
            if (menuname == 0 || menuname > 20)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "名称不合法" });
            }
            if (string.IsNullOrWhiteSpace(model.Menucode))
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "代码不能为空" });
            }
            bool power = true;
            model.Createby = UserInfo.UserCode;
            model.Createdate = DateTime.Now;
            if (Request.Form["isvisible"].ToString() == "0")
            {
                model.Isvisible = false;
            }
            else
            {
                model.Isvisible = true;
            }
            if (Request.Form["isenable"].ToString()=="0")
            {
                model.Isenable = false;
            }
            else
            {
                model.Isenable = true;
            }
            if (!string.IsNullOrWhiteSpace(model.Parentcode))
            {

                model.Menulevel = ISysMenuService.Query(" and menucode='" + model.Parentcode + "'")[0].Menulevel + 1;
            }
            else
            {
                model.Menulevel = 0;
            }
            List<SysMenuFunctionEntity> list = new List<SysMenuFunctionEntity>();
            //处理fun 功能
            if (Request.Form["funcode"] != null )
            {
                var funcode = RequestHelper.GetStringArrayNoNull(Request.Form["funcode"]);
                foreach (var item in funcode)
                {
                    list.Add(new SysMenuFunctionEntity
                    {
                        Funcode = item,
                        Menucode = model.Menucode
                    });
                }
            }
            int result = 0;
            if (model.Id>0)
            {
                power = SysPower(SysMenuConstant.MENU_MANAGE, SysMenuConstant.FUN_UPDATE, UserInfo.UserCode, false);
                if (!power)
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "权限不足" });
                }
                result = ISysMenuService.UpdateMenuFun(model, list);
            }
            else
            {
                power = SysPower(SysMenuConstant.MENU_MANAGE, SysMenuConstant.FUN_ADD, UserInfo.UserCode, false);
                if (!power)
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "权限不足" });
                }
                result = ISysMenuService.InsertMenuFun(model, list);
            }
            if (result > 0)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "保存成功" });
            }
            else
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "保存失败" });
            }
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
       public JsonResult GetAllMenu()
        {
            string menuname = RequestHelper.FilterParam(Request.QueryString["menuname"]);
            //return new CustomerJsonResult(new ResponseResultDataList { code= ResponseHelper.ESUCCESSCODE , msg="请求成功", data= GetMenuAllData(),count=100});
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = GetMenuListAllData(menuname), count = 100 });
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelMenu(int id)
        {
            var r = SysPower(SysMenuConstant.MENU_DICTIONARY_MANAGE, SysMenuConstant.FUN_DELETE, UserInfo.UserCode, false);
            if (!r)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "无删除权限" });
            }
            int result= ISysMenuService.Delete(id.ToString());
            if (result>0)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "删除成功" });
            }
            else
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "删除失败" });
            }
        }

        #endregion

        #region 私用方法
        private List<Hashtable> GetMenuAllData()
        {
            List<Hashtable> lsetm = new List<Hashtable>();
            var listpartmenu = ISysMenuService.Query(string.Empty);
         
            var onelsmenu = listpartmenu.Where(w => w.Menulevel == 0);
            if (onelsmenu != null && onelsmenu.Count() > 0)
            {
                foreach (var item in onelsmenu)
                {
                    Hashtable etm = new Hashtable();
                    etm.Add("id", item.Id);
                    etm.Add("name", item.Menuname);
                    etm.Add("url", item.Menuurl);
                    etm.Add("menuSeq", item.Menuseq);
                    etm.Add("iconCls", item.Menuicon);
                    etm.Add("menuCode", item.Menucode);
                    etm.Add("pId", 0);
                    etm.Add("description", item.Remark);
                    GetChildrenMenu(lsetm, listpartmenu, item.Menucode, item.Id);
                    lsetm.Add(etm);
                }
            }
            return lsetm;
        }

        private void GetChildrenMenu(List<Hashtable> lisths, IList<SysMenuEntity> listpartmenu, string MenuCode,int? pid)
        {
            var children = listpartmenu.Where(w => w.Parentcode == MenuCode);
            if (children != null && children.Count() > 0)
            {
             
                foreach (var item in children)
                {
                    Hashtable etmchildren = new Hashtable();
                    etmchildren.Add("id", item.Id);
                    etmchildren.Add("name", item.Menuname);
                    etmchildren.Add("url", item.Menuurl);
                    etmchildren.Add("menuSeq", item.Menuseq);
                    etmchildren.Add("iconCls", item.Menuicon);
                    etmchildren.Add("menuCode", item.Menucode);
                    etmchildren.Add("description", item.Remark);
                    etmchildren.Add("pId", pid);
                    GetChildrenMenu(lisths, listpartmenu, item.Menucode, item.Id);
                    lisths.Add(etmchildren);
                }
            }
        }

        private List<Hashtable> GetMenuListAllData(string menuName)
        {
            List<Hashtable> lsetm = new List<Hashtable>();
            string strwhere = string.Empty;
            if (!string.IsNullOrEmpty(menuName))
            {
                strwhere = " and menuname like '%" + menuName + "%'";
            }
            IEnumerable<dynamic> menulist =  ISysMenuService.GetMenuList(strwhere);
            foreach (var item in menulist)
            {
                Hashtable etm = new Hashtable();
                etm.Add("id", item.id);
                etm.Add("name", item.menuname);
                etm.Add("url", item.menuurl);
                etm.Add("menuSeq", item.menuseq);
                etm.Add("iconCls", item.menuseq);
                etm.Add("menuCode", item.menucode);
                etm.Add("pId", item.pid);
                etm.Add("description", item.remark);
                etm.Add("funcode", item.funcode);
                lsetm.Add(etm);
            }
            return lsetm;
        }
        
        #endregion
    }
}