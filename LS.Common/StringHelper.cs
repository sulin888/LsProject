using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Common
{
   public class StringHelper
    {
       public static int GetStrLength(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str).Length;
        }
    }
}
