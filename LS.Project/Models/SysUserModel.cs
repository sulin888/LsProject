using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS.Project.Models
{
    public class SysUserModel
    {
        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string OrganizeName { get; set; }

        public string ConfigJson { get; set; }

        public string UserPhoto { get; set; }
    }
}