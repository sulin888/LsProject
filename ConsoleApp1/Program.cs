using LS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string val = "2019-08-07 - 2019-08-08";
            var match= System.Text.RegularExpressions.Regex.Matches(val, @"\d{4}-\d{2}-\d{2}");
            foreach (var item in match)
            {
                Console.WriteLine(item);
            }
            Console.Read();
        }
       
    }
}
