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
    public class DictionaryController : BaseAdminController
    {
        #region OrganizeController
        ISysDictionary ISysDictionaryService;
        public DictionaryController(ISysDictionary repositoryDic)
        {
            ISysDictionaryService = repositoryDic;
            
        }
        #endregion

        #region 页面
        public ActionResult Index()
        {
            SysPower(SysMenuConstant.MENU_DICTIONARY_MANAGE, SysMenuConstant.FUN_SELECT, UserInfo.UserCode);
            return View();
        }
        public ActionResult CreatDictionary()
        {
            SysPower(SysMenuConstant.MENU_DICTIONARY_MANAGE, SysMenuConstant.FUN_ADD, UserInfo.UserCode);
            var model = new SysDictionaryEntity();
            model.Isenable = true;
            return View("EidtDictionary", model);
        }
        public ActionResult EidtDictionary(int id)
        {
            SysPower(SysMenuConstant.MENU_DICTIONARY_MANAGE, SysMenuConstant.FUN_UPDATE, UserInfo.UserCode);
            var model = ISysDictionaryService.GetById(id);
            if (model == null)
            {
                return ErrorCustomMsg(ResponseHelper.NONEXISTENT);
            }
            return View(model);
        }
        #endregion

        #region 公共方法
        [HttpGet]
        public JsonResult GetDictionary()
        {
            string keyword = RequestHelper.FilterParam(Request.QueryString["keyword"]);
           
            int page=   RequestHelper.GetQueryValueOrNull<int>("page");
            int limit = RequestHelper.GetQueryValueOrNull<int>("limit");
            string strwhere = string.Empty;
            if (!string.IsNullOrEmpty(keyword))
            {
                strwhere += " and (dname like '%"+ keyword + "%' or dcode like '%" + keyword + "%' )";
            }
            var result= ISysDictionaryService.GetPageData(strwhere, page, limit);
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = result.Items, count = result.TotalNum });
        }

        [HttpGet]
        public JsonResult GetDictionaryCondition()
        {
            string keyword = RequestHelper.FilterParam(Request.QueryString["keyword"]);
            List<Hashtable> lsetm = new List<Hashtable>();
            string strwhere = " and isenable=1 ";
            if (!string.IsNullOrEmpty(keyword))
            {
                strwhere = " and dname like '%" + keyword + "%'";
            }
            var rolelist = ISysDictionaryService.Query(strwhere);

            foreach (var item in rolelist)
            {
                Hashtable etm = new Hashtable();
                etm.Add("value", item.Dcode);
                etm.Add("name", item.Dname);
                etm.Add("selected", "");
                etm.Add("disabled", "");
                lsetm.Add(etm);
            }
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功",
                data = lsetm, count = lsetm.Count }); ;
        }

        /// <summary>
        /// 保存字典
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveDictionary(SysDictionaryEntity model)
        {
            var menuname = StringHelper.GetStrLength(model.Dname);
            bool power = true;
            if (menuname == 0 || menuname > 30)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "名称不合法" });
            }
            if (string.IsNullOrWhiteSpace(model.Dcode))
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.ORPARAMCODE, msg = "代码不能为空" });
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
           
            int result = 0;
            if (model.Id > 0)
            {
                power = SysPower(SysMenuConstant.MENU_DICTIONARY_MANAGE, SysMenuConstant.FUN_UPDATE, UserInfo.UserCode, false);
                if (!power)
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "权限不足" });
                }
                result = ISysDictionaryService.Update(model);
            }
            else
            {
                power = SysPower(SysMenuConstant.MENU_DICTIONARY_MANAGE, SysMenuConstant.FUN_ADD, UserInfo.UserCode, false);
                if (!power)
                {
                    return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "权限不足" });
                }
                result = ISysDictionaryService.Insert(model);
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
       public JsonResult DelDict(int id)
        {
            var r=SysPower(SysMenuConstant.MENU_DICTIONARY_MANAGE, SysMenuConstant.FUN_DELETE, UserInfo.UserCode,false);
            if (!r)
            {
                return new CustomerJsonResult(new ResponseResult { code = ResponseHelper.FAILCODE, msg = "无删除权限" });
            }
            int result = ISysDictionaryService.Delete(id.ToString());
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

        #endregion

    }
}