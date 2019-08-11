using LS.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Cores
{
    /// <summary>
    /// 角色管理自定义方法
    /// </summary>
    public interface ISysRole : RepositoryBase<SysRoleEntity>
    {
        /// <summary>
        /// 根据角色获取对应的菜单
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        IList<SysRoleMenuEntity> QueryRoleMenu(string strwhere);

        /// <summary>
        /// 保存角色及菜单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="listModel"></param>
        /// <returns></returns>
        int InsertRoleMenu(SysRoleEntity model, List<SysRoleMenuEntity> listModel);

        /// <summary>
        /// 修改角色及菜单
        /// </summary>  
        /// <param name="model"></param>
        /// <param name="listModel"></param>
        /// <returns></returns>
        int UpdateRoleMenu(SysRoleEntity model, List<SysRoleMenuEntity> listModel);
    }
}
