using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace LS.Project.Helper
{
    public class ResponseHelper
    {

        #region 提示消息
        public const string NONEXISTENT= "记录不存在或已删除";

        /// <summary>
        /// 用于验证失败
        /// </summary>
        public const int ORPARAMCODE = 403;

        /// <summary>
        /// 用于成功
        /// </summary>
        public const int ESUCCESSCODE = 0;

        /// <summary>
        /// 用于失败
        /// </summary>
        public const int FAILCODE = 500;
        #endregion
    }
}