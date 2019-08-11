using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 用户概要说明
    /// </summary>
    [Serializable]
    public class SysUserReumeEntity
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
        /// Retype
        /// <summary>
        public int? Retype { get; set; }
        /// <summary>
        /// Beginendyear
        /// <summary>
        public string Beginendyear { get; set; }
        /// <summary>
        /// Content
        /// <summary>
        public string Content { get; set; }
        /// <summary>
        /// Majorduty
        /// <summary>
        public string Majorduty { get; set; }
    }
}
