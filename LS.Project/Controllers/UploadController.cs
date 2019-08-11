using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS.Project.Controllers
{
    public class UploadController : BaseAdminController
    {

        public ActionResult Index()
        {
            return View();
        }

        #region 公共方法
        [HttpPost]
        public JsonResult UploadHeadImg()
        {
            HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count == 0) return Json(new { code = 0, msg = "对象为空" });
            //判断文件夹是否存在
        
            string uploadPath = Server.MapPath(string.Format("~/upload/head/"));

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            //验证后缀名称 
            var file = files[0];
            bool isFlag = IsAllowExtension(file);
            if (isFlag) //验证大小
            {
                if (file.ContentLength > 50 * 1024)
                {
                    return Json(new { code = 1, msg = "超过上传大小限制" });
                }
            }
            else
            {
                return Json(new { code = 1, msg = "不允许上传文件类型" });
            }
            var filename = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
            file.SaveAs(uploadPath + filename);
            return Json(new { code = 0, filename = filename, url= "/upload/head/"+ filename });
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 验证后缀名
        /// </summary>
        /// <returns></returns>
        public bool IsAllowExtension(HttpPostedFile filename)
        {
            if (filename != null)
            {
                string suffix = filename.FileName.Substring(filename.FileName.LastIndexOf("."));
                List<string> listextension = new List<string> {
            ".xls",".xlsx",".doc",".docx",".pdf",".jpg",".gif","",".bmp",".png"
            };
                if (listextension.Contains(suffix.ToLower())) return true;
            }
            return false;


        }
        public static bool IsAllowedExtension(HttpPostedFile file)
        {
            BinaryReader reader = new BinaryReader(file.InputStream);

            Dictionary<string, string> diclist = new Dictionary<string, string> {

            {"208207","doc|xls|ppt|wps"},
            {"8075","docx|pptx|xlsx|zip"},
            {"108101","txt"},
            {"8297","rar"},
            {"7790","exe"},
            {"3780","pdf"},
            {"6033","htm|html"},
            {"255216","jpg"},
            {"7173","gif"},
            {"13780","png"},
            {"6677","bmp"},
            {"64101","bat"}
            };
            string fileclass = "";
            try
            {
                for (int i = 0; i < 2; i++)
                {
                    fileclass += reader.ReadByte().ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
            if (diclist.ContainsKey(fileclass))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}