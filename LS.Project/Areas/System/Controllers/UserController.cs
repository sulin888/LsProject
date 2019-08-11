using System;
using LS.Common;
using LS.Cores;
using LS.Entitys;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LS.Project.Controllers;
using LS.Project.Helper;
using System.Text;
using LS.Project.Filters;

namespace LS.Project.Areas.System.Controllers
{
    public class UserController : BaseAdminController
    {
        #region UserController
        ISysUser ISysUserService;
        public UserController(ISysUser repositoryUser)
        {
            ISysUserService = repositoryUser;
        }
        #endregion

        #region 页面
        
        public ActionResult Index()
        {
            SysPower(SysMenuConstant.MENU_USER_MANAGE, SysMenuConstant.FUN_SELECT, UserInfo.UserCode);
            return View();
        }

        public  ActionResult CreateUser()
        {
            SysPower(SysMenuConstant.MENU_USER_MANAGE, SysMenuConstant.FUN_ADD, UserInfo.UserCode);
            var model = new SysUserEntity();
            model.Isenable = true;
            model.Usercode = RequestHelper.GetTakeCode();
            ViewBag.Photo = "/Content/images/user.png";

            ViewBag.ZwList = ContainerBuilderHelper.Instance.GetDictionary(" and dictype='ZW' ");
            ViewBag.ZjList = ContainerBuilderHelper.Instance.GetDictionary(" and dictype='ZJ' ");
            return View("EditUser", model);
        }
        public ActionResult EditUser(int id)
        {
            SysPower(SysMenuConstant.MENU_USER_MANAGE, SysMenuConstant.FUN_UPDATE, UserInfo.UserCode);
            var model = ISysUserService.GetById(id);
            if (model == null)
            {
                return ErrorCustomMsg(ResponseHelper.NONEXISTENT);
            }
            var  userinfo= ISysUserService.GetUserInfo(" and  usercode='" + model.Usercode + "'");
            if (userinfo != null)
            {
                ViewBag.Realname = userinfo.Realname;
                ViewBag.Photo = userinfo.Photo;
                ViewBag.Political = userinfo.Political;
                ViewBag.Maritalstatus = userinfo.Maritalstatus;
                ViewBag.Birthdate = userinfo.Birthdate;
                ViewBag.Sex = userinfo.Sex;
                ViewBag.Nation = userinfo.Nation;
                ViewBag.Placeorigin = userinfo.Placeorigin;
                ViewBag.Education = userinfo.Education;
                ViewBag.Telephone = userinfo.Telephone;
                ViewBag.University = userinfo.University;
                ViewBag.Specialty = userinfo.Specialty;
                ViewBag.Presentaddress = userinfo.Presentaddress;
                ViewBag.Email = userinfo.Email;
                ViewBag.Hobby = userinfo.Hobby;
                ViewBag.Perspecialty = userinfo.Perspecialty;
                ViewBag.Comprehensive = userinfo.Comprehensive;
                ViewBag.Selfevaluation = userinfo.Selfevaluation;
            }
            var orguser = ISysUserService.GetOrgUser(" and usercode='" + model.Usercode + "'");
            if(orguser!=null && orguser.Count > 0)
            {
                StringBuilder sborg = new StringBuilder();
                foreach (var item in orguser)
                {
                    sborg.Append(item.Orgcode+",");
                }
                ViewBag.OrgCode = RequestHelper.RemoveSuffixChar(sborg.ToString());
            }
            var roleuser = ISysUserService.GetRoleUser(" and usercode='" + model.Usercode + "'");
            if (roleuser != null && roleuser.Count > 0)
            {
                StringBuilder sbrole = new StringBuilder();
                foreach (var item in roleuser)
                {
                    sbrole.Append(item.Rolecode + ",");
                }
                ViewBag.RoleCode = RequestHelper.RemoveSuffixChar(sbrole.ToString());
            }

            ViewBag.ZwList = ContainerBuilderHelper.Instance.GetDictionary(" and dictype='ZW' ");
            ViewBag.ZjList = ContainerBuilderHelper.Instance.GetDictionary(" and dictype='ZJ' ");
            return View(model);
        }

        public ActionResult BaseInfo()
        {
            var model = ISysUserService.Query(" and  usercode='"+ UserInfo.UserCode+"'").FirstOrDefault();
            if (model == null)
            {
                return ErrorCustomMsg(ResponseHelper.NONEXISTENT);
            }
            var userinfo = ISysUserService.GetUserInfo(" and  usercode='" + model.Usercode + "'");
            if (userinfo != null)
            {
                ViewBag.Realname = userinfo.Realname;
                ViewBag.Photo = userinfo.Photo;
                ViewBag.Political = userinfo.Political;
                ViewBag.Maritalstatus = userinfo.Maritalstatus;
                ViewBag.Birthdate = userinfo.Birthdate;
                ViewBag.Sex = userinfo.Sex;
                ViewBag.Nation = userinfo.Nation;
                ViewBag.Placeorigin = userinfo.Placeorigin;
                ViewBag.Education = userinfo.Education;
                ViewBag.Telephone = userinfo.Telephone;
                ViewBag.University = userinfo.University;
                ViewBag.Specialty = userinfo.Specialty;
                ViewBag.Presentaddress = userinfo.Presentaddress;
                ViewBag.Email = userinfo.Email;
                ViewBag.Hobby = userinfo.Hobby;
                ViewBag.Perspecialty = userinfo.Perspecialty;
                ViewBag.Comprehensive = userinfo.Comprehensive;
                ViewBag.Selfevaluation = userinfo.Selfevaluation;
            }
            return View(model);
        }

        public ActionResult ModifyPwd()
        {
            return View();
        }
        #endregion

        #region  公共方法
        [HttpGet]
        public JsonResult GetAllUser()
        {
            string keyword = RequestHelper.FilterParam(Request.QueryString["keyword"]);

            int page = RequestHelper.GetQueryValueOrNull<int>("page");
            int limit = RequestHelper.GetQueryValueOrNull<int>("limit");
            string strwhere = string.Empty;
            if (!string.IsNullOrEmpty(keyword))
            {
                strwhere += " and (dname like '%" + keyword + "%' or dcode like '%" + keyword + "%' )";
            }
            var result = ISysUserService.GetPageData(strwhere, page, limit);
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = result.Items, count = result.TotalNum });
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveUser(SysUserEntity model)
        {
            var menuname = StringHelper.GetStrLength(model.Username);
            bool power = true;
            if (menuname == 0 || menuname > 30)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "名称不合法" });
            }
            if (string.IsNullOrWhiteSpace(model.Usercode))
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "代码不能为空" });
            }
            if (string.Equals("Admin", model.Username, StringComparison.InvariantCultureIgnoreCase))
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "用户名已被占用" });
            }
            string strwhere = " and  username='" + RequestHelper.FilterParam(model.Username) + "'";

            SysUserEntity savemodel = null;
            if (model.Id > 0)
            {
                power = SysPower(SysMenuConstant.MENU_USER_MANAGE, SysMenuConstant.FUN_UPDATE, UserInfo.UserCode, false);
                if (!power)
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "权限不足" });
                }
                savemodel = ISysUserService.GetById(model.Id.Value);
                strwhere += " and  id!=" + model.Id;
            }
            int count = ISysUserService.Query(strwhere).Count;
            if (count > 0)
            {
                power = SysPower(SysMenuConstant.MENU_USER_MANAGE, SysMenuConstant.FUN_ADD, UserInfo.UserCode, false);
                if (!power)
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "权限不足" });
                }
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "用户名已被占用" });
            }
            model.Createby = UserInfo.UserCode;
            model.Createdate = DateTime.Now;

            if (Request.Form["isenable"].ToString() == "0")
            {
                model.Isenable = false;
            }
            else
            {
                model.Isenable = true;
            }

            SysUserinfoEntity userinfo = new SysUserinfoEntity();
            model.JobcodeName = Request.Form["jobName"].ToString();
            model.PositionsName = Request.Form["positName"].ToString();

            #region 获取userinfo 参数
            userinfo.Usercode = model.Usercode;
            userinfo.Realname= RequestHelper.FilterParam(Request.Form["Realname"]);
            userinfo.Photo = RequestHelper.FilterParam(Request.Form["photo"]);
            userinfo.Political = RequestHelper.FilterParam(Request.Form["political"]);
            userinfo.Maritalstatus = RequestHelper.FilterParam(Request.Form["maritalstatus"]);
            if (!string.IsNullOrEmpty(Request.Form["birthdate"]))
            {
                userinfo.Birthdate = Request.Form["birthdate"].ToString().GetValueOrNull<DateTime>();
            }
            userinfo.Sex = RequestHelper.FilterParam(Request.Form["sex"]);
            userinfo.Nation = RequestHelper.FilterParam(Request.Form["nation"]);
            userinfo.Placeorigin = RequestHelper.FilterParam(Request.Form["placeorigin"]);
            userinfo.Education = RequestHelper.FilterParam(Request.Form["education"]);
            userinfo.Telephone = RequestHelper.FilterParam(Request.Form["telephone"]);
            userinfo.University = RequestHelper.FilterParam(Request.Form["university"]);
            userinfo.Specialty = RequestHelper.FilterParam(Request.Form["specialty"]);
            userinfo.Presentaddress = RequestHelper.FilterParam(Request.Form["presentaddress"]);
            userinfo.Email = RequestHelper.FilterParam(Request.Form["email"]);
            userinfo.Hobby = RequestHelper.FilterParam(Request.Form["hobby"]);
            userinfo.Perspecialty = RequestHelper.FilterParam(Request.Form["perspecialty"]);
            userinfo.Comprehensive = RequestHelper.FilterParam(Request.Form["comprehensive"]);
            userinfo.Selfevaluation = RequestHelper.FilterParam(Request.Form["selfevaluation"]);
            userinfo.Selfevaluation = RequestHelper.FilterParam(Request.Form["selfevaluation"]);
            userinfo.Createdate = DateTime.Now;
            userinfo.Createby = UserInfo.UserCode;
            #endregion

            #region 概要说明
            List<SysUserReumeEntity> list = new List<SysUserReumeEntity>();
            string study = Request.Form["BeginEndYearStu"] == null ? "" : Request.Form["BeginEndYearStu"].ToString();
            //学习经历
            if (study != "")
            {
                string stucontent = Request.Form["ContentStu"] == null ? "" : Request.Form["ContentStu"].ToString();
                string stumajorduty = Request.Form["MajorDutyStu"] == null ? "" : Request.Form["MajorDutyStu"].ToString();
                string[] studylist = study.Split(',');
                string[] itemcontent = stucontent.Split(',');
                string[] itemmajor = stumajorduty.Split(',');
                for (int i = 0; i < studylist.Length; i++)
                {
                    list.Add(new SysUserReumeEntity
                    {
                        Usercode = model.Usercode,
                        Retype = 0,
                        Beginendyear = studylist[i],
                        Content = itemcontent[i],
                        Majorduty = itemmajor[i]
                    });
                }

            }
            string work = Request.Form["BeginEndYearWork"] == null ? "" : Request.Form["BeginEndYearWork"].ToString();
            //工作经历
            if (study != "")
            {
                string workcontent = Request.Form["ContentWork"] == null ? "" : Request.Form["ContentWork"].ToString();
                string workmajorduty = Request.Form["MajorDutyWork"] == null ? "" : Request.Form["MajorDutyWork"].ToString();
                string[] worklist = work.Split(',');
                string[] itemcontent = workcontent.Split(',');
                string[] itemmajor = workmajorduty.Split(',');
                for (int i = 0; i < worklist.Length; i++)
                {
                    list.Add(new SysUserReumeEntity
                    {
                        Usercode = model.Usercode,
                        Retype = 1,
                        Beginendyear = worklist[i],
                        Content = itemcontent[i],
                        Majorduty = itemmajor[i]
                    });
                }

            }
            #endregion

            #region 角色
            var listrole = RequestHelper.GetStringListNoNull("userroles");
            List<SysUserroleEntity> lsrole = new List<SysUserroleEntity>();
            if(listrole.Count>0)
            {
                foreach (var item in listrole)
                {
                    lsrole.Add(new SysUserroleEntity {  Rolecode=item,Usercode=model.Usercode});
                }
            }
            #endregion

            #region 组织机构
            var listorg = RequestHelper.GetStringListNoNull("userorgs");
            List<SysOrganizeuserEntity> lsorg = new List<SysOrganizeuserEntity>();
            if (listorg.Count > 0)
            {
                foreach (var item in listorg)
                {
                    lsorg.Add(new SysOrganizeuserEntity { Orgcode = item, Usercode = model.Usercode });
                }
            }
            #endregion
            int result = 0;

            if (model.Id > 0)
            {
                var md5pwd = DESEncryptHelper.GetMd5Hash(model.Userpwd);
                
                if (md5pwd != savemodel.Userpwd)
                {
                    model.Userpwd = md5pwd;
                }
                result = ISysUserService.UpdateUser(model, userinfo, list,lsorg,lsrole);
            }
            else
            {
                model.Userpwd = DESEncryptHelper.GetMd5Hash(model.Userpwd);
                model.Createdate = DateTime.Now;
                model.Createby = UserInfo.UserCode;
                result = ISysUserService.InsertUser(model, userinfo,list, lsorg, lsrole);
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

        [HttpPost]
        public JsonResult DelUser(int id)
        {
            var r = SysPower(SysMenuConstant.MENU_USER_MANAGE, SysMenuConstant.FUN_DELETE, UserInfo.UserCode, false);
            if (!r)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "无删除权限" });
            }
            int result = ISysUserService.Delete(id.ToString());
            if (result > 0)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "删除成功" });
            }
            else
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "删除失败" });
            }
        }

        [HttpPost]
        public JsonResult ModifyEnabled(string uids,bool  enabled)
        {
            var r = SysPower(SysMenuConstant.MENU_USER_MANAGE, SysMenuConstant.FUN_UPDATE, UserInfo.UserCode, false);
            if (!r)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "权限不足" });
            }
            bool result = false;
            if (!string.IsNullOrEmpty(uids))
            {
                int status = enabled ? 1 : 0;
                result=ISysUserService.UpdateEnable(uids, status);
            }
            if (result)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "状态操作成功" });
            }
            else
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "状态操作失败" });
            }
            
            
        }

        [HttpGet]
        public JsonResult GetUserInfo(string userCode)
        {
            string strwhere = string.Empty;
            if (!string.IsNullOrEmpty(userCode))
            {
                strwhere += " and usercode='"+ RequestHelper.FilterParam(userCode) + "'";
            }
          var model=  ISysUserService.GetUserInfo(strwhere);
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = model });
        }
        [HttpGet]
        public JsonResult GetUserResume(string userCode)
        {
            string strwhere = string.Empty;
            if (!string.IsNullOrEmpty(userCode))
            {
                strwhere += " and usercode='" + RequestHelper.FilterParam(userCode) + "'";
            }
            var list = ISysUserService.GetUserResume(strwhere);
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = list });
        }

        [HttpPost]
        public JsonResult ModifyPassWord(string pwd,string againpwd)
        {
            bool result = false;
            if (string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(againpwd))
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "密码和确认密码不能为空" });
            }
            if(pwd!= againpwd)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "两次输入密码不一致" });
            }
            result=ISysUserService.UpdatePwd("UserCode", UserInfo.UserCode, DESEncryptHelper.GetMd5Hash(pwd));
            if (result)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "状态操作成功" });
            }
            else
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "状态操作失败" });
            }
        }
        [HttpPost]
        public JsonResult ExistsUserName(string userName,int? id)
        {
            if (string.Equals("Admin", userName, StringComparison.InvariantCultureIgnoreCase))
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "用户名已被占用" });
            }
            string strwhere = " and  username='" + RequestHelper.FilterParam(userName) + "'";
            if (id.HasValue && id.Value > 0)
            {
                strwhere += " and  id!=" + id;
            }
           int count=   ISysUserService.Query(strwhere).Count;
            if (count>0)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "用户名已被占用" });
            }
            else
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "用户名可用" });
            }
        }
        #endregion

        #region 私有方法

        #endregion
    }
}