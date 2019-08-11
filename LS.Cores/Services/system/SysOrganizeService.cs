using System;
using System.Collections;
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
    /// 提供组织结构管理服务
    /// </summary>
    [Serializable]
    public class SysOrganizeService : ISysOrganize
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
                    string sql = "delete from  sys_organize where id=@Id";
                    r = SqlHelper.ExecuteCon(sql.ToString(), new { Id = id }, tran, null, CommandType.Text, con);

                    string sqlfun = "delete from  sys_organizerole where Orgcode=@Orgcode";
                    r = SqlHelper.ExecuteCon(sqlfun.ToString(), new { Orgcode = model.Orgcode }, tran, null, CommandType.Text, con);

                    r = 1;//标识成功

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
        public SysOrganizeEntity GetById(int id)
        {
            string sql = "select * from  sys_organize where id=@Id";
            return SqlHelper.Query<SysOrganizeEntity>(sql, new { Id = id }).FirstOrDefault();
        }
        public PageDataView<SysOrganizeEntity> GetPageData(string strwhere, int currentPage = 1, int pageSize = 20)
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
            criteria.TableName = "sys_organize";
            criteria.PrimaryKey = "id";
            return SqlHelper.GetPageData<SysOrganizeEntity>(criteria);
        }

        public IList<SysOrganizeEntity> Query(string strwhere)
        {
            string sql = "select * From  sys_organize where 1=1 ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += strwhere;
            }
            return SqlHelper.Query<SysOrganizeEntity>(sql).ToList();
        }
        public int Insert(SysOrganizeEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("insert into sys_organize(orgcode,orgname,parentcode,orgseq,isdel,remark,createby,createdate) ");
            strsql.Append(" values(@orgcode,@orgname,@parentcode,@orgseq,@isdel,@remark,@createby,@createdate)");
            return SqlHelper.Execute(strsql.ToString(), model);
        }
        public int Update(SysOrganizeEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("update sys_organize set  ");
            strsql.Append("orgcode=@orgcode,orgname=@orgname,parentcode=@parentcode,orgseq=@orgseq,isdel=@isdel,remark=@remark,createby=@createby,createdate=@createdate");
            strsql.Append(" where id=@id ");
            return SqlHelper.Execute(strsql.ToString(), model);
        }


        #endregion 实现接口方法

        public int InsertOrgRole(SysOrganizeEntity model, List<SysOrganizeroleEntity> listModel)
        {
            bool resulte = SqlHelper.ExecuteTranSql((con, tran) =>
            {
                int r = 0;
                try
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("insert into sys_organize(orgcode,orgname,parentcode,orgseq,isdel,remark,createby,createdate) ");
                    strsql.Append(" values(@orgcode,@orgname,@parentcode,@orgseq,@isdel,@remark,@createby,@createdate)");
                    r = SqlHelper.ExecuteCon(strsql.ToString(), model, tran, null, CommandType.Text, con);

                    if (listModel != null && listModel.Count > 0)
                    {
                        StringBuilder strsqlfun = new StringBuilder();
                        strsqlfun.Append("insert into sys_organizerole(orgcode,rolecode) ");
                        strsqlfun.Append(" values(@orgcode,@rolecode)");
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

        public int UpdateOrgRole(SysOrganizeEntity model, List<SysOrganizeroleEntity> listModel)
        {
            bool resulte = SqlHelper.ExecuteTranSql((con, tran) =>
            {
                int r = 0;
                try
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("update sys_organize set  ");
                    strsql.Append("orgcode=@orgcode,orgname=@orgname,parentcode=@parentcode,orgseq=@orgseq,isdel=@isdel,remark=@remark,createby=@createby,createdate=@createdate");
                    strsql.Append(" where id=@id ");
                    r = SqlHelper.ExecuteCon(strsql.ToString(), model, tran, null, CommandType.Text, con);
                    string sqlfun = "delete from  sys_organizerole where orgcode=@Orgcode";
                    r = SqlHelper.ExecuteCon(sqlfun.ToString(), new { Orgcode = model.Orgcode }, tran, null, CommandType.Text, con);
                    StringBuilder strsqlfun = new StringBuilder();
                    if (listModel != null && listModel.Count > 0)
                    {
                        strsqlfun.Append("insert into sys_organizerole(orgcode,rolecode) ");
                        strsqlfun.Append(" values(@orgcode,@rolecode)");
                        r = SqlHelper.ExecuteCon(strsqlfun.ToString(), listModel, tran, null, CommandType.Text, con);
                    }
                    r = 1;//标识成功
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

        public Hashtable GetMaxPartOrg(string orgCode)
        {
            Hashtable hs = new Hashtable();
            string sql = "select isnull(MAX(orgcode),'') OrgCode ,isnull(max(orgseq),0) OrgSeq  From sys_organize ";
            if (string.IsNullOrEmpty(orgCode))
            {
                sql += " where parentcode is null ";
            }
            else
            {
                sql += " where ParentCode='" + orgCode + "' ";
            }
            var model = SqlHelper.Query(sql).FirstOrDefault();
            foreach (var it in model)
            {
                if (it.Key == "OrgCode")
                {
                    hs.Add("OrgCode", it.Value);
                }
                if (it.Key == "OrgSeq")
                {
                    hs.Add("OrgSeq", it.Value);
                }

            }
            return hs;
        }

        public IEnumerable<dynamic> GetOrganizeList(string strwhere)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select A.*,isnull((select top 1 id from sys_organize where orgcode=A.parentcode),0) pid,");
            strsql.Append("dbo.f_organizerole(A.orgcode) as orgnames ");
            strsql.Append("from sys_organize A ");
            if (!string.IsNullOrEmpty(strwhere))
            {
                strsql.Append(" where 1=1 " + strwhere);
            }
            strsql.Append("order by orgcode,parentcode,orgseq");
            return SqlHelper.Query<dynamic>(strsql.ToString());
        }

        public IList<SysOrganizeroleEntity> GetOrganizeRoleList(string strwhere)
        {
            string sql = "select * From  sys_organizerole where 1=1 ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += strwhere;
            }
            return SqlHelper.Query<SysOrganizeroleEntity>(sql).ToList();
        }
    }
}
