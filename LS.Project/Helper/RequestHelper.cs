using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace LS.Project.Helper
{
    public static class RequestHelper
    {
        /// <summary>
        /// 获取客户端真实Ip
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }

        /// <summary>
        /// 获取当前系统的域名、IP
        /// </summary>
        /// <returns></returns>
        public static string GetCurrectDomain()
        {
            Uri currectUri = HttpContext.Current.Request.Url;
            StringBuilder hostName = new StringBuilder(currectUri.Scheme + "://");
            hostName.Append(currectUri.Host);
            if (currectUri.Port != 80)
            {
                hostName.Append(":" + currectUri.Port);
            }
            return hostName.ToString();
        }
        /// <summary>
        /// 转换类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="valueAsString"></param>
        /// <returns></returns>
        public static T? GetValueOrNull<T>(this string valueAsString) where T : struct
        {
            if (string.IsNullOrEmpty(valueAsString))
                return null;
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }
        public static T GetQueryValueOrNull<T>(string key)
        {
            var val = HttpContext.Current.Request[key];
            if (string.IsNullOrEmpty(val))
                return default(T);
            return (T)Convert.ChangeType(val, typeof(T));
        }
        /// <summary>
        /// 转换为string[]
        /// </summary>
        /// <param name="valueAsString"></param>
        /// <returns></returns>
        public static string[] GetStringArrayNoNull(this string valueAsString)
        {

            if (!string.IsNullOrEmpty(valueAsString))
            {
                return valueAsString.Split(',');
            }

            return new string[] { };
        }
        public static List<string> GetStringListNoNull(string key)
        {
            var val = HttpContext.Current.Request[key];
            List<string> result = new List<string>();
            if (!string.IsNullOrEmpty(val))
            {
                return val.Split(',').ToList<string>();
            }

            return result;
        }
        /// <summary>
        /// 检查危险字符
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string FilterParam(string sInput)
        {
            if (sInput == null || sInput == "")
                return null;
            string sInput1 = sInput.ToLower();
            string output = sInput;
            string pattern = @"*|and|exec|insert|select|delete|update|count|master|truncate|declare|char(|mid(|chr(|'";
            //if (Regex.Match(sInput1, Regex.Escape(pattern), RegexOptions.Compiled | RegexOptions.IgnoreCase).Success)
            //{
            //    throw new Exception("字符串中含有非法字符!");
            //}
            //else
            //{
            output = output.Replace("'", "''");
            //}
            return output;
        }


        /// <summary>
        /// 获取8位不重复的数字
        /// </summary>
        /// <returns></returns>
        public static string GetTakeCode()
        {
            byte[] randomBytes = new byte[4];
            RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
            rngServiceProvider.GetBytes(randomBytes);
            Int32 result = BitConverter.ToInt32(randomBytes, 0);
            string takecode = result.ToString().Replace("-", "");
            if (takecode.Length > 8)
            {
                return takecode.Substring(0, 8);
            }
            else
            {
                return GetTakeCode();
            }
        }


        /// <summary>
        /// 逗号分割的字符转成sql的字符串
        /// </summary>
        /// <param name="strSplit"></param>
        /// <returns></returns>
        public static string GetStrSql(string strSplit)
        {
            if (string.IsNullOrEmpty(strSplit))
            {
                return string.Empty;
            }
            string[] aryval = strSplit.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder strsql = new StringBuilder();
            for (int i = 0; i < aryval.Length; i++)
            {
                strsql.Append("'" + aryval[i] + "',");
            }
            return System.Text.RegularExpressions.Regex.Replace(strsql.ToString(), ",$", "");
        }
        /// <summary>
        /// 去掉字符串末尾逗号字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSuffixChar(string str)
        {
            return System.Text.RegularExpressions.Regex.Replace(str, ",$", "");
        }

        public static Dictionary<string,string> GetSystemConfig()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
               
                string path = HttpContext.Current.Server.MapPath("/Configs/system.config");
                var xmlDocument = XDocument.Load(path);
                var appSettings = xmlDocument.Elements("appSettings").Elements();
                foreach (var item in appSettings)
                {
                  
                    dic.Add(item.Attribute("key").Value, item.Attribute("value").Value);
                }


            }
            catch (Exception ex)
            {
               
            }
            return dic;
        }

        public static string GetSystemConfigByKey(string key)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            
            try
            {

                string path = HttpContext.Current.Server.MapPath("/Configs/system.config");
                var xmlDocument = XDocument.Load(path);
                var appSettings = xmlDocument.Elements("appSettings").Elements();
                foreach (var item in appSettings)
                {
                    if (item.Attribute("key").Value== key)
                    {
                        return item.Attribute("value").Value;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return string.Empty ;
        }

        /// <summary>
        /// 格式化获取日期
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static List<string> GetDateForStr(string dateStr )
        {
            List<string> datels = new List<string>();
            var matchcoll= System.Text.RegularExpressions.Regex.Matches(dateStr, @"\d{4}-\d{2}-\d{2}");
            foreach (var item in matchcoll)
            {
                datels.Add(item.ToString());
            }
            return datels;
        }

    }
}