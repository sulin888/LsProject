using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 角色菜单管理
    /// </summary>
    [Serializable]
    public class SysRoleMenuEntity
    {
        /// <summary>
        /// Id
        /// <summary>
        public int? Id { get; set; }
        /// <summary>
        /// Menucode
        /// <summary>
        public string Menucode { get; set; }
        /// <summary>
        /// Rolecode
        /// <summary>
        public string Rolecode { get; set; }
        /// <summary>
        /// Funcode
        /// <summary>
        public string Funcode { get; set; }
    }
}
