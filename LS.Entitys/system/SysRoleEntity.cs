using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Serializable]
    public class SysRoleEntity
    {
        /// <summary>
        /// Id
        /// <summary>
        public int? Id { get; set; }
        /// <summary>
        /// Rolecode
        /// <summary>
        public string Rolecode { get; set; }
        /// <summary>
        /// Rolename
        /// <summary>
        public string Rolename { get; set; }
        /// <summary>
        /// Roleseq
        /// <summary>
        public int? Roleseq { get; set; }
        /// <summary>
        /// Remark
        /// <summary>
        public string Remark { get; set; }
        /// <summary>
        /// Createby
        /// <summary>
        public string Createby { get; set; }
        /// <summary>
        /// Createdate
        /// <summary>
        public DateTime? Createdate { get; set; }
    }
}
