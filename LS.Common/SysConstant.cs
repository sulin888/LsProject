using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Common
{
    public sealed class SysConstant
    {
        /// <summary>
        /// 用户登录保存session/cookie 的key
        /// </summary>
        public const string SEESIONUSERKEY = "UserInfo";
        /// <summary>
        /// 用户扫描登录
        /// </summary>
        public const string SEESIONQRCODE = "QRCode";
    }
}
