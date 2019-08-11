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
    /// 组织结构管理自定义方法
    /// </summary>
    public interface ISysOrganize : RepositoryBase<SysOrganizeEntity>
    {
        /// <summary>
        /// 保存组织结构管理
        /// </summary>
        /// <param name="model"></param>
        /// <param name="listModel"></param>
        /// <returns></returns>
        int InsertOrgRole(SysOrganizeEntity model, List<SysOrganizeroleEntity> listModel);

        /// <summary>
        /// 修改组织结构管理
        /// <param name="model"></param>
        /// <param name="listModel"></param>
        /// <returns></returns>
        int UpdateOrgRole(SysOrganizeEntity model, List<SysOrganizeroleEntity> listModel);

        /// <summary>
        /// 获取自动菜单编号
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        Hashtable GetMaxPartOrg(string orgCode);

        /// <summary>
        /// 获取组织机构
        /// </summary>
        /// <returns></returns>
        IEnumerable<dynamic> GetOrganizeList(string strwhere);

        /// <summary>
        /// 根据条件获取对应的关系
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        IList<SysOrganizeroleEntity> GetOrganizeRoleList(string strwhere);
    }
}
