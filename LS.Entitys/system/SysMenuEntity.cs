using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [Serializable]
    public class SysMenuEntity
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
        /// Parentcode
        /// <summary>
        public string Parentcode { get; set; }
        /// <summary>
        /// Menuname
        /// <summary>
        public string Menuname { get; set; }
        /// <summary>
        /// Menuurl
        /// <summary>
        public string Menuurl { get; set; }
        /// <summary>
        /// Menuicon
        /// <summary>
        public string Menuicon { get; set; }
        /// <summary>
        /// Menuseq
        /// <summary>
        public int? Menuseq { get; set; }
        /// <summary>
        /// Isvisible
        /// <summary>
        public bool? Isvisible { get; set; }
        /// <summary>
        /// Isenable
        /// <summary>
        public bool? Isenable { get; set; }
        /// <summary>
        /// Menulevel
        /// <summary>
        public int? Menulevel { get; set; }
        /// <summary>
        /// Createby
        /// <summary>
        public string Createby { get; set; }
        /// <summary>
        /// Createdate
        /// <summary>
        public DateTime? Createdate { get; set; }
        /// <summary>
        /// Remark
        /// <summary>
        public string Remark { get; set; }


        public string Rolecode { get; set; }
        public string Funcode { get; set; }
    }
}
