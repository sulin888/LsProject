using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 用户角色表
    /// </summary>
    [Serializable]
    public class SysUserroleEntity
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
        /// Rolecode
        /// <summary>
        public string Rolecode { get; set; }
    }
}
