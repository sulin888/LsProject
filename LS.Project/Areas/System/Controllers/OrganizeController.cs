using LS.Common;
using LS.Cores;
using LS.Entitys;
using LS.Project.Controllers;
using LS.Project.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS.Project.Areas.System.Controllers
{
    public class OrganizeController : BaseAdminController
    {

        #region OrganizeController
        ISysRole ISysRoleService;
        ISysOrganize ISysOrgService;
        public OrganizeController(ISysRole repositoryRole, ISysOrganize repositoryOrganize)
        {
            ISysRoleService = repositoryRole;
            ISysOrgService = repositoryOrganize;
        }
        #endregion

        #region 页面
        public ActionResult Index()
        {
            SysPower(SysMenuConstant.MENU_ORGANIZE_MANAGE, SysMenuConstant.FUN_SELECT, UserInfo.UserCode);
            return View();
        }
        public ActionResult CreateOrg()
        {
            SysPower(SysMenuConstant.MENU_ORGANIZE_MANAGE, SysMenuConstant.FUN_ADD, UserInfo.UserCode);
            var model = new SysOrganizeEntity();
            #region 增加是自动获取代码和序列号
            string strwhere = string.Empty;

            Hashtable hs = ISysOrgService.GetMaxPartOrg(RequestHelper.FilterParam(Request.QueryString["code"]));
            string parentCode = "";
            int menseq = 0;
            if (hs.Count > 0)
            {
                parentCode = hs["OrgCode"].ToString();
                menseq = Convert.ToInt32(hs["OrgSeq"]);
            }
            string code = RequestHelper.GetQueryValueOrNull<string>("code");
            string currcode = "";
            if (parentCode == "")
            {
                if (string.IsNullOrEmpty(code))//表示顶级菜单
                {
                    currcode = "201";
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
            model.Orgcode = currcode;
            model.Orgseq = menseq;
            model.Parentcode = code;
            #endregion
            return View("EidtOrg", model);
        }
        public ActionResult EidtOrg(int id)
        {
            SysPower(SysMenuConstant.MENU_ORGANIZE_MANAGE, SysMenuConstant.FUN_UPDATE, UserInfo.UserCode);
            var model = ISysOrgService.GetById(id);
            if (model == null)
            {
                return ErrorCustomMsg(ResponseHelper.NONEXISTENT);
            }
            var orgrolelist= ISysOrgService.GetOrganizeRoleList(" and  orgcode='"+ model .Orgcode+ "'");
           
            ViewBag.OrgRoleList =string.Join(",", orgrolelist.Select(s => "'"+s.Rolecode+"'").ToArray());
            return View(model);
        }
        #endregion

        #region 公共方法
        [HttpGet]
        public JsonResult GetAllOrganize()
        {
            string rolename = RequestHelper.FilterParam(Request.QueryString["orgname"]);
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = GetOrganizeAllData(rolename), count = 100 });
        }
        [HttpGet]
        public JsonResult GetAllTreeOrganize()
        {
            string orgcode = RequestHelper.FilterParam(Request.QueryString["orgcode"]);
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = GetOrganizeTreeAllData(orgcode), count = 100 });
        }
        /// <summary>
        /// 保存组织机构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveOrgRole(SysOrganizeEntity model)
        {
            var orgname = StringHelper.GetStrLength(model.Orgname);
            bool power = true;
            if (orgname == 0 || orgname > 20)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "名称不合法" });
            }
            if (string.IsNullOrWhiteSpace(model.Orgcode))
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "代码不能为空" });
            }
            model.Createby = UserInfo.UserCode;
            model.Createdate = DateTime.Now;

            List<SysOrganizeroleEntity> list = new List<SysOrganizeroleEntity>();
            //处理角色 功能
            var listrolecode = RequestHelper.GetStringListNoNull("rolecode");
            foreach (var item in listrolecode)
            {
                list.Add(new SysOrganizeroleEntity
                {
                    Orgcode = model.Orgcode,
                    Rolecode = item
                });
            }
            
            int result = 0;
            if (model.Id > 0)
            {
                power = SysPower(SysMenuConstant.MENU_ORGANIZE_MANAGE, SysMenuConstant.FUN_UPDATE, UserInfo.UserCode, false);
                if (!power)
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "权限不足" });
                }
                result = ISysOrgService.UpdateOrgRole(model, list);
            }
            else
            {
                power = SysPower(SysMenuConstant.MENU_ORGANIZE_MANAGE, SysMenuConstant.FUN_ADD, UserInfo.UserCode, false);
                if (!power)
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "权限不足" });
                }
                result = ISysOrgService.InsertOrgRole(model, list);
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
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DelOrg(int id)
        {
            var r = SysPower(SysMenuConstant.MENU_ORGANIZE_MANAGE, SysMenuConstant.FUN_DELETE, UserInfo.UserCode, false);
            if (!r)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "无删除权限" });
            }
            int result = ISysOrgService.Delete(id.ToString());
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
        private List<Hashtable> GetOrganizeAllData(string orgname)
        {
            List<Hashtable> lsetm = new List<Hashtable>();
            string strwhere = string.Empty;
            if (!string.IsNullOrEmpty(orgname))
            {
                strwhere = " and orgname like '%" + orgname + "%'";
            }
            IEnumerable<dynamic>  rolelist = ISysOrgService.GetOrganizeList(strwhere);

            foreach (var item in rolelist)
            {
                Hashtable etm = new Hashtable();
                etm.Add("id", item.id);
                etm.Add("pId", item.pid);
                etm.Add("name", item.orgname);
                etm.Add("orgSeq", item.orgseq);
                etm.Add("orgCode", item.orgcode);
                etm.Add("description", item.remark);
                etm.Add("orgnames", item.orgnames);
                lsetm.Add(etm);
            }
            return lsetm;
        }
        private List<Hashtable> GetOrganizeTreeAllData(string orgCode)
        {
            List<Hashtable> lsetm = new List<Hashtable>();
            string strwhere = string.Empty;
            string[] orgarry = RequestHelper.GetStringArrayNoNull(orgCode);
            IEnumerable<dynamic> rolelist = ISysOrgService.GetOrganizeList(strwhere);

            foreach (var item in rolelist)
            {
                Hashtable etm = new Hashtable();
                etm.Add("id", item.id);
                etm.Add("pId", item.pid);
                etm.Add("name", item.orgname);
                etm.Add("code", item.orgcode);
                string orgcode = item.orgcode;
                if (orgarry.Contains(orgcode))
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
        private string GetRoleMenuFun(List<string> listfuncode, string menucode)
        {
            List<string> ls = new List<string>();
            foreach (var item in listfuncode)
            {
                var mf = item.Split('-');
                if (mf[0] == menucode)
                {
                    ls.Add(mf[1]);
                }
            }
            return string.Join(",", ls);
            ;
        }
        #endregion

    }
}