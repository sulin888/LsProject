using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS.Project.Models
{
    public class SysMenuModel
    {
        public int id { get; set; }

        public string text { get; set; }

        public string iconCls { get; set; }
        public List<SysMenuModel> children { get; set; }

        public AttributesModel attributes { get; set; }
    }
    public class AttributesModel
    {
        public string datalink { get; set; }

        public string iframe { get; set; }

    }
}