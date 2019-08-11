using LS.Common;
using LS.Cores;
using LS.Entitys;
using LS.Project.Controllers;
using LS.Project.Helper;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LS.Project.Areas.System.Controllers
{
    public class RoleController : BaseAdminController
    {

        #region MenuController
        ISysRole ISysRoleService;
        ISysFunction ISysFunctionService;
        ISysMenu ISysMenuService;
        public RoleController(ISysRole repositoryRole, ISysFunction repositoryFun, ISysMenu repositoryMenu)
        {
            ISysRoleService = repositoryRole;
            ISysFunctionService = repositoryFun;
            ISysMenuService = repositoryMenu;
        }
        #endregion

        #region 页面
        public ActionResult Index()
        {
            SysPower(SysMenuConstant.MENU_ROLE_MANAGE, SysMenuConstant.FUN_SELECT, UserInfo.UserCode);
            return View();
        }

        public ActionResult CreateRole()
        {
            SysPower(SysMenuConstant.MENU_ROLE_MANAGE, SysMenuConstant.FUN_ADD, UserInfo.UserCode);
            var model = new SysRoleEntity();
            ViewBag.MenuFunList = JsonConvert.SerializeObject(ISysFunctionService.Query(string.Empty));

            return View("EditRole", model);
        }

        public ActionResult EditRole(int id)
        {
            SysPower(SysMenuConstant.MENU_ROLE_MANAGE, SysMenuConstant.FUN_UPDATE, UserInfo.UserCode);
            var model = ISysRoleService.GetById(id);
            if (model == null)
            {
                return ErrorCustomMsg(ResponseHelper.NONEXISTENT);
            }
            ViewBag.MenuFunList = JsonConvert.SerializeObject(ISysFunctionService.Query(string.Empty));
            return View(model);
        }
        #endregion



        #region 公共方法
        [HttpGet]
        public JsonResult GetAllRole()
        {
            string rolename = RequestHelper.FilterParam(Request.QueryString["rolename"]);
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = GetRoleListAllData(rolename), count = 100 });
        }
        [HttpGet]
        public JsonResult GetTreeAllRole()
        {
            string rolecode = RequestHelper.FilterParam(Request.QueryString["rolecode"]);
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = GetRoleListAllTreeData(rolecode), count = 100 });
        }
        [HttpGet]
        public JsonResult GetAllRoleCondition()
        {
            string rolename = RequestHelper.FilterParam(Request.QueryString["keyword"]);
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = GetRoleCondition(rolename), count = 100 });
        }
        /// <summary>
        /// 保存菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveRoleMenu(SysRoleEntity model)
        {
            var menuname = StringHelper.GetStrLength(model.Rolename);
            bool power = true;
            if (menuname == 0 || menuname > 20)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "名称不合法" });
            }
            if (string.IsNullOrWhiteSpace(model.Rolecode))
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "代码不能为空" });
            }
            model.Createby = UserInfo.UserCode;
            model.Createdate = DateTime.Now;
           
            List<SysRoleMenuEntity> list = new List<SysRoleMenuEntity>();
            //处理菜单 功能
            var listfuncode = RequestHelper.GetStringListNoNull("funcode");
            if (Request.Form["menucode"] != null)
            {
                var funcode = RequestHelper.GetStringArrayNoNull(Request.Form["menucode"]);
                foreach (var item in funcode)
                {
                    list.Add(new SysRoleMenuEntity
                    {
                        Menucode = item,
                        Rolecode = model.Rolecode,
                        Funcode = GetRoleMenuFun(listfuncode, item)
                    }) ;
                }
            }
            int result = 0;
            if (model.Id > 0)
            {
                power = SysPower(SysMenuConstant.MENU_ROLE_MANAGE, SysMenuConstant.FUN_UPDATE, UserInfo.UserCode, false);
                if (!power)
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "权限不足" });
                }
                result = ISysRoleService.UpdateRoleMenu(model, list);
            }
            else
            {
                power = SysPower(SysMenuConstant.MENU_ROLE_MANAGE, SysMenuConstant.FUN_ADD, UserInfo.UserCode, false);
                if (!power)
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "权限不足" });
                }
                result = ISysRoleService.InsertRoleMenu(model, list);
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
        public JsonResult GetRoleAllMenu()
        {
            string rolecode = RequestHelper.FilterParam(Request.QueryString["roleCode"]);
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = GetMenuListAllData(rolecode), count = 100 });
        }
        
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelRole(int id)
        {
            var r = SysPower(SysMenuConstant.MENU_ROLE_MANAGE, SysMenuConstant.FUN_DELETE, UserInfo.UserCode, false);
            if (!r)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "无删除权限" });
            }
            int result = ISysRoleService.Delete(id.ToString());
            if (result > 0)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "删除成功" });
            }
            else
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "删除失败" });
            }
        }
        #endregion

        #region 私有方法
        private List<Hashtable> GetMenuListAllData(string roleCode)
        {
            List<Hashtable> lsetm = new List<Hashtable>();
            string strwhere = string.Empty;
           
            IEnumerable<dynamic> menulist = ISysMenuService.GetMenuList(strwhere);
            IList<SysRoleMenuEntity> rolemenu = null;
            if (!string.IsNullOrEmpty(roleCode))
            {
                strwhere = " and rolecode='"+ roleCode + "'";
                rolemenu = ISysRoleService.QueryRoleMenu(strwhere);
            }
           
          
            foreach (var item in menulist)
            {
                Hashtable etm = new Hashtable();
                etm.Add("id", item.id);
                etm.Add("name", item.menuname);
                etm.Add("menuCode", item.menucode);
                etm.Add("pId", item.pid);
                etm.Add("funcode", item.funcode);
                if(rolemenu!=null && rolemenu.Where(w=>w.Menucode== item.menucode).Count()>0)
                {
                    //etm.Add("LAY_CHECKED", true);
                    etm.Add("checked", true);
                    var children = rolemenu.Where(w => w.Menucode == item.menucode);
                    StringBuilder sbch = new StringBuilder();
                    foreach (var chd in children)
                    {
                        sbch.Append(chd.Funcode + ",");
                    }
                    etm.Add("rolefuncode", sbch.ToString());

                }
                lsetm.Add(etm);
            }
            return lsetm;
        }

        private List<Hashtable> GetRoleListAllData(string roleName)
        {
            List<Hashtable> lsetm = new List<Hashtable>();
            string strwhere = string.Empty;
            if (!string.IsNullOrEmpty(roleName))
            {
                strwhere= " and rolename like '%" + roleName + "%'";
            }
            var rolelist = ISysRoleService.Query(strwhere);

            foreach (var item in rolelist)
            {
                Hashtable etm = new Hashtable();
                etm.Add("id", item.Id);
                etm.Add("name", item.Rolename);
                etm.Add("roleSeq", item.Roleseq);
                etm.Add("roleCode", item.Rolecode);
                etm.Add("description", item.Remark);
                lsetm.Add(etm);
            }
            return lsetm;
        }

        private List<Hashtable> GetRoleListAllTreeData(string roleCode)
        {
            List<Hashtable> lsetm = new List<Hashtable>();
            string[] rolearry = RequestHelper.GetStringArrayNoNull(roleCode);
            var rolelist = ISysRoleService.Query(string.Empty);

            foreach (var item in rolelist)
            {
                Hashtable etm = new Hashtable();
                etm.Add("id", item.Rolecode);
                etm.Add("name", item.Rolename);
                if (rolearry.Contains(item.Rolecode))
                {
                    etm.Add("checked", true);
                }
                lsetm.Add(etm);
            }
            return lsetm;
        }
        /// <summary>
        /// 获取菜单功能关联
        /// </summary>
        /// <param name="menufun"></param>
        /// <returns></returns>
        private string GetRoleMenuFun(List<string> listfuncode,string menucode)
        {
            List<string> ls = new List<string>();
            foreach (var item in listfuncode)
            {
                var mf = item.Split('-');
                if (mf[0]== menucode)
                {
                    ls.Add(mf[1]);
                }
            }
            return string.Join(",", ls);
;
        }

        private List<Hashtable> GetRoleCondition(string rolename)
        {
            List<Hashtable> lsetm = new List<Hashtable>();
            string strwhere = string.Empty;
            if (!string.IsNullOrEmpty(rolename))
            {
                strwhere = " and rolename like '%" + rolename + "%'";
            }
            var rolelist = ISysRoleService.Query(strwhere);

            foreach (var item in rolelist)
            {
                Hashtable etm = new Hashtable();
                etm.Add("value", item.Rolecode);
                etm.Add("name", item.Rolename);
                etm.Add("selected", "");
                etm.Add("disabled", "");
                lsetm.Add(etm);
            }
            return lsetm;
        }
        #endregion

    }
}