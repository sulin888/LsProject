using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 日志管理
    /// </summary>
    [Serializable]
    public class SysLogsEntity
    {
        /// <summary>
        /// Id
        /// <summary>
        public int? Id { get; set; }
        /// <summary>
        /// Logtype  0=信息、1=错误、2=提醒、3=调试
        /// <summary>
        public int? Logtype { get; set; }
        /// <summary>
        /// Logsource
        /// <summary>
        public string Logsource { get; set; }
        /// <summary>
        /// Logmsg
        /// <summary>
        public string Logmsg { get; set; }
        /// <summary>
        /// Createtime
        /// <summary>
        public DateTime? Createtime { get; set; }
    }
}
