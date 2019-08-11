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
    /// 提供角色管理服务
    /// </summary>
    [Serializable]
    public class SysRoleService : ISysRole
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
                    string sql = "delete from  sys_role where id=@Id";
                    r = SqlHelper.ExecuteCon(sql.ToString(), new { Id = id }, tran, null, CommandType.Text, con);

                    string sqlfun = "delete from  sys_rolemenu where rolecode=@Rolecode";
                    r = SqlHelper.ExecuteCon(sqlfun.ToString(), new { Rolecode = model.Rolecode }, tran, null, CommandType.Text, con);

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
        public SysRoleEntity GetById(int id)
        {
            string sql = "select * from  sys_role where id=@Id";
            return SqlHelper.Query<SysRoleEntity>(sql, new { Id = id }).FirstOrDefault();
        }
        public PageDataView<SysRoleEntity> GetPageData(string strwhere, int currentPage = 1, int pageSize = 20)
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
            criteria.TableName = "sys_role";
            criteria.PrimaryKey = "id";
            return SqlHelper.GetPageData<SysRoleEntity>(criteria);
        }
        public IList<SysRoleEntity> Query(string strwhere)
        {
            string sql = "select * From  sys_role where 1=1 ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += strwhere;
            }
            return SqlHelper.Query<SysRoleEntity>(sql).ToList();
        }
        public int Insert(SysRoleEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("insert into sys_role(rolecode,rolename,roleseq,remark,createby,createdate) ");
            strsql.Append(" values(@rolecode,@rolename,@roleseq,@remark,@createby,@createdate)");
            return SqlHelper.Execute(strsql.ToString(), model);
        }
        public int Update(SysRoleEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("update sys_role set  ");
            strsql.Append("rolecode=@rolecode,rolename=@rolename,roleseq=@roleseq,remark=@remark,createby=@createby,createdate=@createdate");
            strsql.Append(" where id=@id ");
            return SqlHelper.Execute(strsql.ToString(), model);
        }


        #endregion 实现接口方法

        public IList<SysRoleMenuEntity> QueryRoleMenu(string strwhere)
        {
            string sql = "select * From  sys_rolemenu where 1=1 ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += strwhere;
            }
            return SqlHelper.Query<SysRoleMenuEntity>(sql).ToList();
        }

        public int InsertRoleMenu(SysRoleEntity model, List<SysRoleMenuEntity> listModel)
        {
            bool resulte = SqlHelper.ExecuteTranSql((con, tran) =>
            {
                int r = 0;
                try
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("insert into sys_role(rolecode,rolename,roleseq,remark,createby,createdate) ");
                    strsql.Append(" values(@rolecode,@rolename,@roleseq,@remark,@createby,@createdate)");

                    r = SqlHelper.ExecuteCon(strsql.ToString(), model, tran, null, CommandType.Text, con);

                    if (listModel != null && listModel.Count > 0)
                    {
                        StringBuilder strsqlfun = new StringBuilder();
                        strsqlfun.Append("insert into sys_rolemenu(menucode,rolecode,funcode) ");
                        strsqlfun.Append(" values(@menucode,@rolecode,@funcode)");
                        r = SqlHelper.ExecuteCon(strsqlfun.ToString(), listModel, tran, null, CommandType.Text, con);
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

        public int UpdateRoleMenu(SysRoleEntity model, List<SysRoleMenuEntity> listModel)
        {
            bool resulte = SqlHelper.ExecuteTranSql((con, tran) =>
            {
                int r = 0;
                try
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("update sys_role set  ");
                    strsql.Append("rolecode=@rolecode,rolename=@rolename,roleseq=@roleseq,remark=@remark,createby=@createby,createdate=@createdate");
                    strsql.Append(" where id=@id ");
                    r = SqlHelper.ExecuteCon(strsql.ToString(), model, tran, null, CommandType.Text, con);

                    if (listModel != null && listModel.Count > 0)
                    {
                        string sqlrolemenu = "delete from  sys_rolemenu where rolecode=@Rolecode";
                        r = SqlHelper.ExecuteCon(sqlrolemenu.ToString(), new { Rolecode = model.Rolecode }, tran, null, CommandType.Text, con);
                        StringBuilder strsqlfun = new StringBuilder();
                        strsqlfun.Append("insert into sys_rolemenu(menucode,rolecode,funcode) ");
                        strsqlfun.Append(" values(@menucode,@rolecode,@funcode)");
                        r = SqlHelper.ExecuteCon(strsqlfun.ToString(), listModel, tran, null, CommandType.Text, con);
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
    }
}
