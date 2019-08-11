using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Cores
{
  public  class BaseCore
    {
        //基类配置 如多个数据库切换等
        /// <summary>
        /// 接口日志数据库
        /// </summary>
        public static ConnectionStringSettings CON_MYSQL_PLATLOG = ConfigurationManager.ConnectionStrings["MYSQL_LOG"];

    }
}
