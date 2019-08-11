using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 菜单功能管理
    /// </summary>
    [Serializable]
    public class SysMenuFunctionEntity
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
        /// Funcode
        /// <summary>
        public string Funcode { get; set; }
    }
}
