using LS.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LS.Cores
{
    /// <summary>
    /// 用户表管理自定义方法
    /// </summary>
    public interface ISysUser : RepositoryBase<SysUserEntity>
    {
        /// <summary>
        /// 增加用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="listModel"></param>
        /// <returns></returns>
        int InsertUser(SysUserEntity model, SysUserinfoEntity userinfoModel,List<SysUserReumeEntity> userreumeList,
            List<SysOrganizeuserEntity> orguserList, List<SysUserroleEntity> userroleList);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="listModel"></param>
        /// <returns></returns>
        int UpdateUser(SysUserEntity model, SysUserinfoEntity userinfoModel, List<SysUserReumeEntity> userreumeList,
            List<SysOrganizeuserEntity> orguserList, List<SysUserroleEntity> userroleList);
        /// <summary>
        /// 获取用户详细概要说明
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        List<SysUserReumeEntity> GetUserResume(string strwhere);
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="uids"></param>
        /// <param name="enable">0=禁用 1= 可用</param>
        /// <returns></returns>
        bool UpdateEnable(string uids,int enable);

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        SysUserinfoEntity GetUserInfo(string strwhere);

        /// <summary>
        /// 机构用户关系
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        List<SysOrganizeuserEntity> GetOrgUser(string strwhere);

        /// <summary>
        /// 机构用户关系
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        List<SysUserroleEntity> GetRoleUser(string strwhere);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="key">[Id,UserCode]</param>
        ///<param name="val"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        bool UpdatePwd(string key, string val, string pwd);


    }
}
