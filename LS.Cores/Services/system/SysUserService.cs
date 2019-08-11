using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Common;
using LS.Entitys;
namespace LS.Cores
{
    /// <summary>
    /// 提供用户表管理服务
    /// </summary>
    [Serializable]
    public class SysUserService : ISysUser
    {
        #region 实现接口方法
        public int Delete(string id)
        {
            var model = GetById(Convert.ToInt32(id));
            bool resulte = SqlHelper.ExecuteTranSql((con, tran) =>
            {
                int r = 0;
                try
                {
                    string sql = "delete from  sys_user where id=@Id";
                    SqlHelper.ExecuteCon(sql.ToString(), new { Id = id }, tran, null, CommandType.Text, con);

                    string sqlfun = "delete from  sys_userinfo where usercode=@Usercode";
                    SqlHelper.ExecuteCon(sqlfun.ToString(), new { Usercode = model.Usercode }, tran, null, CommandType.Text, con);

                    string sqlrole = "delete from  sys_userrole where usercode=@Usercode";
                    SqlHelper.ExecuteCon(sqlrole.ToString(), new { Usercode = model.Usercode }, tran, null, CommandType.Text, con);

                    string sqlorg = "delete from  sys_organizeuser where usercode=@Usercode";
                    SqlHelper.ExecuteCon(sqlorg.ToString(), new { Usercode = model.Usercode }, tran, null, CommandType.Text, con);
                    r = 1;
                }
                catch (Exception ex)
                {
                    Log4NetHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).Debug(ex.Message);
                    r = 0;
                }

                return r;
            });
            return resulte ? 1 : 0;
        }
        public SysUserEntity GetById(int id)
        {
            string sql = "select * from  sys_user where id=@Id";
            return SqlHelper.Query<SysUserEntity>(sql, new { Id = id }).FirstOrDefault();
        }
        public PageDataView<SysUserEntity> GetPageData(string strwhere, int currentPage = 1, int pageSize = 20)
        {
            PageCriteria criteria = new PageCriteria();
            criteria.Condition = "1=1";
            if (!string.IsNullOrEmpty(strwhere))
            {
                criteria.Condition += strwhere;
            }
            criteria.CurrentPage = currentPage;
            criteria.Fields = "*";
            criteria.PageSize = pageSize;
            criteria.TableName = "sys_user";
            criteria.PrimaryKey = "id";
            return SqlHelper.GetPageData<SysUserEntity>(criteria);
        }
        public IList<SysUserEntity> Query(string strwhere)
        {
            string sql = "select * From  sys_user where 1=1 ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += strwhere;
            }
            return SqlHelper.Query<SysUserEntity>(sql).ToList();
        }
        public int Insert(SysUserEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("insert into sys_user(usercode,username,userpwd,rolenames,orgnames,configjson,isenable,createby,createdate,jobcode,positions) ");
            strsql.Append(" values(@usercode,@username,@userpwd,@rolenames,@orgnames,@configjson,@isenable,@createby,@createdate,@jobcode,@positions)");
            return SqlHelper.Execute(strsql.ToString(), model);
        }
        public int Update(SysUserEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("update sys_user set  ");
            strsql.Append("usercode=@usercode,username=@username,userpwd=@userpwd,rolenames=@rolenames,orgnames=@orgnames,configjson=@configjson,isenable=@isenable,createby=@createby,createdate=@createdate,jobcode=@jobcode,positions=@positions");
            strsql.Append(" where id=@id ");
            return SqlHelper.Execute(strsql.ToString(), model);
        }


        #endregion 实现接口方法
        public int InsertUser(SysUserEntity model, SysUserinfoEntity userinfoModel, List<SysUserReumeEntity> userreumeList,
            List<SysOrganizeuserEntity> orguserList, List<SysUserroleEntity> userroleList)
        {
            bool resulte = SqlHelper.ExecuteTranSql((con, tran) =>
            {
                int r = 0;
                try
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("insert into sys_user(usercode,username,userpwd,rolenames,orgnames,configjson,isenable,createby,createdate,jobcode,positions,jobcodeName,positionsName) ");
                    strsql.Append(" values(@usercode,@username,@userpwd,@rolenames,@orgnames,@configjson,@isenable,@createby,@createdate,@jobcode,@positions,@jobcodeName,@positionsName)");

                    r = SqlHelper.ExecuteCon(strsql.ToString(), model, tran, null, CommandType.Text, con);

                    StringBuilder sqluserinfo = new StringBuilder();
                    sqluserinfo.Append("insert into sys_userinfo(usercode,realname,sex,birthdate,nation,political,maritalstatus,presentaddress,placeorigin,education,university,specialty,hobby,perspecialty,comprehensive,telephone,email,photo,selfevaluation,createby,createdate) ");
                    sqluserinfo.Append(" values(@usercode,@realname,@sex,@birthdate,@nation,@political,@maritalstatus,@presentaddress,@placeorigin,@education,@university,@specialty,@hobby,@perspecialty,@comprehensive,@telephone,@email,@photo,@selfevaluation,@createby,@createdate)");
                    r = SqlHelper.ExecuteCon(sqluserinfo.ToString(), userinfoModel, tran, null, CommandType.Text, con);
                    if (userreumeList != null && userreumeList.Count > 0)
                    {
                        StringBuilder strsqlresume = new StringBuilder();
                        strsqlresume.Append("insert into sys_userresume(usercode,retype,beginendyear,content,majorduty) ");
                        strsqlresume.Append(" values(@usercode,@retype,@beginendyear,@content,@majorduty)");
                        r = SqlHelper.ExecuteCon(strsqlresume.ToString(), userreumeList, tran, null, CommandType.Text, con);
                    }
                    if (orguserList != null && orguserList.Count > 0)
                    {
                        StringBuilder strsqlorguser = new StringBuilder();
                        strsqlorguser.Append("insert into sys_organizeuser(orgcode,usercode) ");
                        strsqlorguser.Append(" values(@orgcode,@usercode)");
                        r = SqlHelper.ExecuteCon(strsqlorguser.ToString(), orguserList, tran, null, CommandType.Text, con);
                    }
                    if (userroleList != null && userroleList.Count > 0)
                    {
                        StringBuilder strsqluserrole = new StringBuilder();
                        strsqluserrole.Append("insert into sys_userrole(usercode,rolecode) ");
                        strsqluserrole.Append(" values(@usercode,@rolecode)");
                        r = SqlHelper.ExecuteCon(strsqluserrole.ToString(), userroleList, tran, null, CommandType.Text, con);

                    }

                }
                catch (Exception ex)
                {
                    Log4NetHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).Debug(ex.Message);
                    r = 0;
                }

                return r;
            });
            return resulte ? 1 : 0;
        }

        public int UpdateUser(SysUserEntity model, SysUserinfoEntity userinfoModel, List<SysUserReumeEntity> userreumeList,
            List<SysOrganizeuserEntity> orguserList, List<SysUserroleEntity> userroleList)
        {
            bool resulte = SqlHelper.ExecuteTranSql((con, tran) =>
            {
                int r = 0;
                try
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("update sys_user set  ");
                    strsql.Append("usercode=@usercode,username=@username,userpwd=@userpwd,jobcode=@jobcode,rolenames=@rolenames,positions=@positions,orgnames=@orgnames,configjson=@configjson,isenable=@isenable,createby=@createby,createdate=@createdate,jobcodeName=@jobcodeName,positionsName=@positionsName");
                    strsql.Append(" where id=@id ");
                    r = SqlHelper.ExecuteCon(strsql.ToString(), model, tran, null, CommandType.Text, con);

                    StringBuilder sqluserinfo = new StringBuilder();
                    sqluserinfo.Append("update sys_userinfo set  ");
                    sqluserinfo.Append("realname=@realname,sex=@sex,birthdate=@birthdate,nation=@nation,political=@political,maritalstatus=@maritalstatus,presentaddress=@presentaddress,placeorigin=@placeorigin,education=@education,university=@university,specialty=@specialty,hobby=@hobby,perspecialty=@perspecialty,comprehensive=@comprehensive,telephone=@telephone,email=@email,photo=@photo,selfevaluation=@selfevaluation,createby=@createby,createdate=@createdate");
                    sqluserinfo.Append(" where usercode=@usercode ");
                    r = SqlHelper.ExecuteCon(sqluserinfo.ToString(), userinfoModel, tran, null, CommandType.Text, con);
                    if (userreumeList != null && userreumeList.Count > 0)
                    {
                        r = SqlHelper.ExecuteCon("delete from  sys_userresume where usercode=@Usercode", new { Usercode = model.Usercode }, tran, null, CommandType.Text, con);
                        StringBuilder strsqlresume = new StringBuilder();
                        strsqlresume.Append("insert into sys_userresume(usercode,retype,beginendyear,content,majorduty) ");
                        strsqlresume.Append(" values(@usercode,@retype,@beginendyear,@content,@majorduty)");
                        r = SqlHelper.ExecuteCon(strsqlresume.ToString(), userreumeList, tran, null, CommandType.Text, con);
                    }
                    if (orguserList != null && orguserList.Count > 0)
                    {
                        r = SqlHelper.ExecuteCon("delete from  sys_organizeuser where usercode=@Usercode", new { Usercode = model.Usercode }, tran, null, CommandType.Text, con);
                        StringBuilder strsqlorguser = new StringBuilder();
                        strsqlorguser.Append("insert into sys_organizeuser(orgcode,usercode) ");
                        strsqlorguser.Append(" values(@orgcode,@usercode)");
                        r = SqlHelper.ExecuteCon(strsqlorguser.ToString(), orguserList, tran, null, CommandType.Text, con);
                    }
                    if (userroleList != null && userroleList.Count > 0)
                    {
                        r = SqlHelper.ExecuteCon("delete from  sys_userrole where usercode=@Usercode", new { Usercode = model.Usercode }, tran, null, CommandType.Text, con);
                        StringBuilder strsqluserrole = new StringBuilder();
                        strsqluserrole.Append("insert into sys_userrole(usercode,rolecode) ");
                        strsqluserrole.Append(" values(@usercode,@rolecode)");
                        r = SqlHelper.ExecuteCon(strsqluserrole.ToString(), userroleList, tran, null, CommandType.Text, con);

                    }
                }
                catch (Exception ex)
                {
                    Log4NetHelper.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).Debug(ex.Message);
                    r = 0;
                }

                return r;
            });
            return resulte ? 1 : 0;
        }

        public List<SysUserReumeEntity> GetUserResume(string strwhere)
        {
            string sql = "select * From  sys_userresume  ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += " where 1=1 " + strwhere;
            }
            return SqlHelper.Query<SysUserReumeEntity>(sql).ToList();
        }

        public bool UpdateEnable(string uids, int enable)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("update sys_user set ");
            strsql.Append(" isenable=@isenable");
            strsql.Append(" where id in (@Id)");
            int result = SqlHelper.Execute(strsql.ToString(), new { isenable = enable, Id = uids });
            return result > 0 ? true : false;
        }

        public SysUserinfoEntity GetUserInfo(string strwhere)
        {
            string sql = "select * From  sys_userinfo  ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += " where 1=1 " + strwhere;
            }
            return SqlHelper.Query<SysUserinfoEntity>(sql).FirstOrDefault();
        }

        public List<SysOrganizeuserEntity> GetOrgUser(string strwhere)
        {
            string sql = "select * From  sys_organizeuser  ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += " where 1=1 " + strwhere;
            }
            return SqlHelper.Query<SysOrganizeuserEntity>(sql).ToList();
        }

        public List<SysUserroleEntity> GetRoleUser(string strwhere)
        {
            string sql = "select * From  sys_userrole  ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += " where 1=1 " + strwhere;
            }
            return SqlHelper.Query<SysUserroleEntity>(sql).ToList();
        }

        public bool UpdatePwd(string key,string val, string pwd)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("update sys_user set ");
            strsql.Append(" userpwd=@userpwd");
            if(key== "Id")
            {
                strsql.Append(" where id =@Id");
            }
            else
            {
                strsql.Append(" where usercode =@Id");
            }
            int result = SqlHelper.Execute(strsql.ToString(), new { userpwd = pwd, Id = val });
            return result > 0 ? true : false;
        }
    }
}
