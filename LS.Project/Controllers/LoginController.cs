using LS.Common;
using LS.Cores;
using LS.Project.Helper;
using LS.Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS.Project.Controllers
{
    public class LoginController : Controller
    {
        #region LoginController
        ISysUser ISysUserService;
        //构造器注入  
        public LoginController(ISysUser repositoryUser)
        {
            ISysUserService = repositoryUser;
        }
        #endregion


        public ActionResult Index()
        {
            return View();
        }
        #region 公共方法
        /// <summary>
        /// 登录操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Loging()
        {
            string name = Request.Form["name"].ToString();
            string pwd = Request.Form["pwd"].ToString();
            string strwhere = string.Empty;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pwd))
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "账号密码不能为口空" });
            }
            else
            {
                strwhere = " and  username='" + RequestHelper.FilterParam(name) + "'";
            }
            var dic = RequestHelper.GetSystemConfig();

            if (name== "Admin")
            {
                if(dic["Admin"]== DESEncryptHelper.GetMd5Hash(pwd))
                {
                    SysUserModel suser = new SysUserModel
                    {
                        UserCode = "ADMIN",
                        UserName = "Admin",
                        RoleName = "管理员",
                        UserPhoto = "/Content/images/head.png"
                    };

                    SetUserLogin(dic["LoginProvider"], suser);
                    ContainerBuilderHelper.Instance.AddLog(new Entitys.SysLogsEntity
                    {
                        Createtime = DateTime.Now,
                        Logtype = 1,
                        Logmsg = suser.UserName + "[" + suser.UserCode + "]登录成功",
                        Logsource = "Login/Loging"
                    });
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "登录成功" });
                }
                else
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "账号或密码不正确" });
                }
            }
            else
            {
                var model = ISysUserService.Query(strwhere).FirstOrDefault();

                if (model != null && model.Username == name && model.Userpwd == DESEncryptHelper.GetMd5Hash(pwd))
                {
                   var userinfo= ISysUserService.GetUserInfo(" and usercode='" + model.Usercode + "'");
                    SysUserModel suser = new SysUserModel
                    {
                        UserCode = model.Usercode,
                        UserName = model.Username,
                        RoleName = model.Rolenames,
                        UserPhoto = userinfo.Photo
                    };
                    SetUserLogin(dic["LoginProvider"], suser);
                    ContainerBuilderHelper.Instance.AddLog(new Entitys.SysLogsEntity {
                         Createtime =DateTime.Now ,
                         Logtype= 1,
                         Logmsg= suser.UserName + "["+ suser .UserCode+ "]登录成功",
                         Logsource= "Login/Loging"
                    });
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ESUCCESSCODE, msg = "登录成功" });

                }
                else
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "账号或密码不正确" });
                }
            }
        }

        /// <summary>
        /// 登录退出
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Loginout()
        {
            var loginProvider = RequestHelper.GetSystemConfigByKey("LoginProvider");
            if(loginProvider== "Cookie")
            {
                CookieHelper.ClearCookie(SysConstant.SEESIONUSERKEY);
            }
            else
            {
                SessionHelper.ClearSession();
            }
            return View("Index");
        }
        #endregion


        #region 私用方法
        private  void SetUserLogin(string loginProvider, SysUserModel user)
        {
            if (loginProvider == "Cookie")
            {
                CookieHelper.SetCookie(SysConstant.SEESIONUSERKEY,DESEncryptHelper.Encrypt( Newtonsoft.Json.JsonConvert.SerializeObject(user)).Replace("+", "%2B"));
            }
            else
            {
                Session[SysConstant.SEESIONUSERKEY] = user;
            }
        }
        #endregion

    }
}