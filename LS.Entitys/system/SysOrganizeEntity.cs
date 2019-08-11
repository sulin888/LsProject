using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 组织结构管理
    /// </summary>
    [Serializable]
    public class SysOrganizeEntity
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
        /// Orgname
        /// <summary>
        public string Orgname { get; set; }
        /// <summary>
        /// Parentcode
        /// <summary>
        public string Parentcode { get; set; }
        /// <summary>
        /// Orgseq
        /// <summary>
        public int? Orgseq { get; set; }
        /// <summary>
        /// Isdel
        /// <summary>
        public bool? Isdel { get; set; }
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
