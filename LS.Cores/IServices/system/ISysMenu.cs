using LS.Entitys;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Cores
{
    /// <summary>
    /// 菜单管理自定义方法
    /// </summary>
    public interface ISysMenu : RepositoryBase<SysMenuEntity>
    {
        /// <summary>
        /// 根据条件获取对应的关系
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
         IList<SysMenuFunctionEntity> GetMenuFunction(string strwhere);
        /// <summary>
        /// 保存菜单及功能
        /// </summary>
        /// <param name="model"></param>
        /// <param name="listModel"></param>
        /// <returns></returns>
        int InsertMenuFun(SysMenuEntity model,List<SysMenuFunctionEntity> listModel);

        /// <summary>
        /// 修改菜单及功能
        /// </summary>  
        /// <param name="model"></param>
        /// <param name="listModel"></param>
        /// <returns></returns>
        int UpdateMenuFun(SysMenuEntity model, List<SysMenuFunctionEntity> listModel);
        /// <summary>
        /// 获取自动菜单编号
        /// </summary>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        Hashtable GetMaxPartMenu(string menuCode);

        /// <summary>
        /// 系统超级管理员获取主菜单下面的所有子菜单
        /// </summary>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        List<SysMenuEntity> GetUserPartMenuAdmin(string menuCode,int mlevel);

        /// <summary>
        /// 获取主菜单下面的子菜单
        /// </summary>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        List<SysMenuEntity> GetUserPartMenu(string userCode,int mlevel);

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        IEnumerable<dynamic> GetMenuList(string strwhere);
    }
}
