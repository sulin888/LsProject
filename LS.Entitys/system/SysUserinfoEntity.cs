using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Entitys
{
    /// <summary>
    /// 用户详情管理
    /// </summary>
    [Serializable]
    public class SysUserinfoEntity
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
        /// Realname
        /// <summary>
        public string Realname { get; set; }
        /// <summary>
        /// Sex
        /// <summary>
        public string Sex { get; set; }
        /// <summary>
        /// Birthdate
        /// <summary>
        public DateTime? Birthdate { get; set; }
        /// <summary>
        /// Nation
        /// <summary>
        public string Nation { get; set; }
        /// <summary>
        /// Political
        /// <summary>
        public string Political { get; set; }
        /// <summary>
        /// Maritalstatus
        /// <summary>
        public string Maritalstatus { get; set; }
        /// <summary>
        /// Presentaddress
        /// <summary>
        public string Presentaddress { get; set; }
        /// <summary>
        /// Placeorigin
        /// <summary>
        public string Placeorigin { get; set; }
        /// <summary>
        /// Education
        /// <summary>
        public string Education { get; set; }
        /// <summary>
        /// University
        /// <summary>
        public string University { get; set; }
        /// <summary>
        /// Specialty
        /// <summary>
        public string Specialty { get; set; }
        /// <summary>
        /// Hobby
        /// <summary>
        public string Hobby { get; set; }
        /// <summary>
        /// Perspecialty
        /// <summary>
        public string Perspecialty { get; set; }
        /// <summary>
        /// Comprehensive
        /// <summary>
        public string Comprehensive { get; set; }
        /// <summary>
        /// Telephone
        /// <summary>
        public string Telephone { get; set; }
        /// <summary>
        /// Email
        /// <summary>
        public string Email { get; set; }
        /// <summary>
        /// Photo
        /// <summary>
        public string Photo { get; set; }
        /// <summary>
        /// Selfevaluation
        /// <summary>
        public string Selfevaluation { get; set; }
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
