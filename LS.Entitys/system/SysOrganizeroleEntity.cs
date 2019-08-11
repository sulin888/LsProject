using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 组织机构角色管理
    /// </summary>
    [Serializable]
    public class SysOrganizeroleEntity
    {
        /// <summary>
        /// Id
        /// <summary>
        public int? Id { get; set; }
        /// <summary>
        /// Orgcode
        /// <summary>
        public string Orgcode { get; set; }
        /// <summary>
        /// Rolecode
        /// <summary>
        public string Rolecode { get; set; }
    }
}
