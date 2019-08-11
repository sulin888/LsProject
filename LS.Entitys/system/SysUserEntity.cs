using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 用户表管理
    /// </summary>
    [Serializable]
    public class SysUserEntity
    {
        /// <summary>
        /// Id
        /// <summary>
        public int? Id { get; set; }
        /// <summary>
        /// Usercode
        /// <summary>
        public string Usercode { get; set; }
        /// <summary>
        /// Username
        /// <summary>
        public string Username { get; set; }
        /// <summary>
        /// Userpwd
        /// <summary>
        public string Userpwd { get; set; }
        /// <summary>
        /// Jobcode
        /// <summary>
        public string Jobcode { get; set; }
        /// <summary>
        /// Rolenames
        /// <summary>
        public string Rolenames { get; set; }
        /// <summary>
        /// Positions
        /// <summary>
        public string Positions { get; set; }
        /// <summary>
        /// Orgnames
        /// <summary>
        public string Orgnames { get; set; }
        /// <summary>
        /// Configjson
        /// <summary>
        public string Configjson { get; set; }
        /// <summary>
        /// Isenable
        /// <summary>
        public bool? Isenable { get; set; }
        /// <summary>
        /// Createby
        /// <summary>
        public string Createby { get; set; }
        /// <summary>
        /// Createdate
        /// <summary>
        public DateTime? Createdate { get; set; }
        /// <summary>
        /// 职务
        /// </summary>
        public string JobcodeName { get; set; }

        /// <summary>
        /// 职级
        /// </summary>
        public string PositionsName { get; set; }
    }
}
