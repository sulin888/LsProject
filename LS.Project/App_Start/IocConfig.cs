using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace LS.Project
{
    public class IocConfig
    {
        public static void RegisterDependencies()
        {


            // 注册组件
            var builder = new ContainerBuilder();
            var services = Assembly.Load("LS.Cores");

            // 注册单个实例
            //builder.RegisterInstance(new MovieRepository()).As<IMovieRepository>();
            //builder.RegisterType <HomeController>();

            //注册controlls实例
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).PropertiesAutowired();

            // 扫描assembly中的组件
            builder.RegisterAssemblyTypes(services)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces();

            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}