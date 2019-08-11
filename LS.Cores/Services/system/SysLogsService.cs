using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Entitys;
namespace LS.Cores
{
    /// <summary>
    /// 提供日志管理服务
    /// </summary>
    [Serializable]
    public class SysLogsService : ISysLogs
    {
        #region 实现接口方法
        public int Delete(string id)
        {
            string sql = "delete from  sys_logs where id=@Id";
            return SqlHelper.Execute(sql, new { Id = id });
        }
        public SysLogsEntity GetById(int id)
        {
            string sql = "select * from  sys_logs where id=@Id";
            return SqlHelper.Query<SysLogsEntity>(sql, new { Id = id }).FirstOrDefault();
        }
        public PageDataView<SysLogsEntity> GetPageData(string strwhere, int currentPage = 1, int pageSize = 20)
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
            criteria.TableName = "sys_logs";
            criteria.PrimaryKey = "id";
            return SqlHelper.GetPageData<SysLogsEntity>(criteria);
        }
        public IList<SysLogsEntity> Query(string strwhere)
        {
            string sql = "select * From  sys_logs where 1=1 ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += strwhere;
            }
            return SqlHelper.Query<SysLogsEntity>(sql).ToList();
        }
        public int Insert(SysLogsEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("insert into sys_logs(logtype,logsource,logmsg,createtime) ");
            strsql.Append(" values(@logtype,@logsource,@logmsg,@createtime)");
            return SqlHelper.Execute(strsql.ToString(), model);
        }
        public int Update(SysLogsEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("update sys_logs set  ");
            strsql.Append("logtype=@logtype,logsource=@logsource,logmsg=@logmsg,createtime=@createtime");
            strsql.Append(" where id=@id ");
            return SqlHelper.Execute(strsql.ToString(), model);
        }
        #endregion 实现接口方法
    }
}
