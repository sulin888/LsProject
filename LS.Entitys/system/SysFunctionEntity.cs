using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 功能项管理
    /// </summary>
    [Serializable]
    public class SysFunctionEntity
    {
        /// <summary>
        /// Id
        /// <summary>
        public int? Id { get; set; }
        /// <summary>
        /// Funcode
        /// <summary>
        public string Funcode { get; set; }
        /// <summary>
        /// Funname
        /// <summary>
        public string Funname { get; set; }
        /// <summary>
        /// Funseq
        /// <summary>
        public int? Funseq { get; set; }
        /// <summary>
        /// Funicon
        /// <summary>
        public string Funicon { get; set; }
        /// <summary>
        /// Remark
        /// <summary>
        public string Remark { get; set; }
    }
}
