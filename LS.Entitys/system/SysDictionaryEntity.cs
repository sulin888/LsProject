using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 字典管理
    /// </summary>
    [Serializable]
    public class SysDictionaryEntity
    {
        /// <summary>
        /// Id
        /// <summary>
        public int? Id { get; set; }
        /// <summary>
        /// Dcode
        /// <summary>
        public string Dcode { get; set; }
        /// <summary>
        /// Parentcode
        /// <summary>
        public string Parentcode { get; set; }
        /// <summary>
        /// Dname
        /// <summary>
        public string Dname { get; set; }
        /// <summary>
        /// Dseq
        /// <summary>
        public int? Dseq { get; set; }
        /// <summary>
        /// Isenable
        /// <summary>
        public bool? Isenable { get; set; }
        /// <summary>
        /// Dictype
        /// <summary>
        public string Dictype { get; set; }
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
