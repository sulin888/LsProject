using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS.Project
{
    public class CustomerJsonResult:JsonResult
    {
        /// <summary>
        /// 自定义输出json 对象
        /// </summary>
        /// <param name="data">ResponseResult/ResponseResultList</param>
        public CustomerJsonResult(object data)
        {
            ObjData = data;
        }
        public object ObjData { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var json = JsonConvert.SerializeObject(ObjData,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    //ContractResolver = new CamelCasePropertyNamesContractResolver(),//小驼峰命名法
                   
                    DateFormatString = "yyyy-MM-dd HH:mm:ss"
                }
                );
            context.HttpContext.Response.Write(json);
        }
    }

    public class ResponseResult
    {
        public int code { get; set; }
        public string msg { get; set; }
    }
    public class ResponseResultList
    {
        public int code { get; set; }
        public string msg { get; set; }

        public object result { get; set;}
    }
    public class ResponseResultDataList
    {
        public int code { get; set; }
        public string msg { get; set; }

        public object data { get; set; }

        public int count { get; set; }
    }

}