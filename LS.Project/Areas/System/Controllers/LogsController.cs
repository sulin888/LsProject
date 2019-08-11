using LS.Cores;
using LS.Project.Controllers;
using LS.Project.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS.Project.Areas.System.Controllers
{
    public class LogsController : BaseAdminController
    {
        #region LogsController
        ISysLogs ISysLogsService;
        public LogsController(ISysLogs repositoryLogs)
        {
            ISysLogsService = repositoryLogs;
        }
        #endregion

        #region 页面
        public ActionResult Index()
        {
            SysPower(SysMenuConstant.MENU_LOGS_MANAGE, SysMenuConstant.FUN_SELECT, UserInfo.UserCode);
            return View();
        }
        #endregion
        #region  公共方法
        [HttpGet]
        public JsonResult GetAllLogs()
        {
            string logsource = RequestHelper.FilterParam(Request.QueryString["logsource"]);
            int logtype = RequestHelper.GetQueryValueOrNull<int>("logtype");
            string logmsg = RequestHelper.FilterParam(Request.QueryString["logmsg"]);
            string createtime = RequestHelper.FilterParam(Request.QueryString["createtime"]);

            int page = RequestHelper.GetQueryValueOrNull<int>("page");
            int limit = RequestHelper.GetQueryValueOrNull<int>("limit");
            string strwhere = string.Empty;
            if (!string.IsNullOrEmpty(logsource))
            {
                strwhere += " and logsource like '%" + logsource + "%' ";
            }
            if (logtype>0)
            {
                strwhere += " and logtype = "+ logtype;
            }
            if (!string.IsNullOrEmpty(logmsg))
            {
                strwhere += " and logmsg ='"+ logmsg + "' " ;
            }
            if (!string.IsNullOrEmpty(createtime))
            {
                var ls=RequestHelper.GetDateForStr(createtime);
                if (ls.Count >= 2)
                {
                    strwhere += " and createtime >'" + ls[0] + "' and createtime<= '" + ls[1] + " 23:59:59' ";
                }
               
            }
            var result = ISysLogsService.GetPageData(strwhere, page, limit);
            return new CustomerJsonResult(new ResponseResultDataList { code = ResponseHelper.ESUCCESSCODE, msg = "请求成功", data = result.Items, count = result.TotalNum });
        }
        #endregion

    }
}