using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 组织机构用户表
    /// </summary>
    [Serializable]
    public class SysOrganizeuserEntity
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
        /// Usercode
        /// <summary>
        public string Usercode { get; set; }
    }
}
