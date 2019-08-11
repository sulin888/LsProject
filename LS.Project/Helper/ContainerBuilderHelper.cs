using Autofac;
using LS.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS.Project.Helper
{
    /// <summary>
    /// ContainerBuilder 帮助类
    /// </summary>
    public class ContainerBuilderHelper
    {
        private static ContainerBuilderHelper _instance;
        /// <summary>
        /// 组建注册容器
        /// </summary>
        IContainer container;
        public static ContainerBuilderHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ContainerBuilderHelper();
                }
                return _instance;
            }
        }

        public ContainerBuilderHelper()
        {
            Autofac.ContainerBuilder builder = new Autofac.ContainerBuilder();
            builder.RegisterType<LS.Cores.SysLogsService>().As<LS.Cores.ISysLogs>();
            builder.RegisterType<LS.Cores.SysMenuService>().As<LS.Cores.ISysMenu>();
            builder.RegisterType<LS.Cores.SysDictionaryService>().As<LS.Cores.ISysDictionary>();
            container = builder.Build();
        }
        /// <summary>
        /// 添加日志信息
        /// </summary>
        /// <param name="apiLogdata"></param>
        /// <returns></returns>
        public int AddLog(SysLogsEntity apiLogdata)
        {
            return container.Resolve<LS.Cores.ISysLogs>().Insert(apiLogdata);
        }
        /// <summary>
        /// 根据用户名code获取菜单
        /// </summary>
        /// <param name="userCode">用户名称</param>
        /// <returns></returns>
        public IList<SysMenuEntity> GetUserPartMenu(string userCode)
        {
            IList<SysMenuEntity> menulist = null;
            var iSysMenu=  container.Resolve<LS.Cores.ISysMenu>();
            if ("ADMIN" == userCode)//管理员
            {
                menulist = iSysMenu.Query(string.Empty);
            }
            else
            {
                menulist = iSysMenu.GetUserPartMenu(userCode, 0);
            }
            return menulist;
        }

        public IList<SysDictionaryEntity> GetDictionary(string strwhere)
        {
            IList<SysDictionaryEntity> listdic = null;
              var dicservice =container.Resolve<LS.Cores.ISysDictionary>();
            if (!string.IsNullOrEmpty(strwhere))
            {
                listdic = dicservice.Query(strwhere);
            }
            return listdic;
        }
    }
}