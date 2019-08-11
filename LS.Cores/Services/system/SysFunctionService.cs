using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Entitys;
namespace LS.Cores
{
    /// <summary>
    /// 提供功能项管理服务
    /// </summary>
    [Serializable]
    public class SysFunctionService : ISysFunction
    {
        #region 实现接口方法
        public int Delete(string id)
        {
            string sql = "delete from  sys_function where id=@Id";
            return SqlHelper.Execute(sql, new { Id = id });
        }
        public SysFunctionEntity GetById(int id)
        {
            string sql = "select * from  sys_function where id=@Id";
            return SqlHelper.Query<SysFunctionEntity>(sql, new { Id = id }).FirstOrDefault();
        }
        public PageDataView<SysFunctionEntity> GetPageData(string strwhere, int currentPage = 1, int pageSize = 20)
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
            criteria.TableName = "sys_function";
            criteria.PrimaryKey = "id";
            return SqlHelper.GetPageData<SysFunctionEntity>(criteria);
        }

        public IList<SysFunctionEntity> Query(string strwhere)
        {
            string sql = "select * From  sys_function where 1=1 ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += strwhere;
            }
            return SqlHelper.Query<SysFunctionEntity>(sql).ToList();
        }
        public int Insert(SysFunctionEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("insert into sys_function(funcode,funname,funseq,funicon,remark) ");
            strsql.Append(" values(@funcode,@funname,@funseq,@funicon,@remark)");
            return SqlHelper.Execute(strsql.ToString(), model);
        }
        public int Update(SysFunctionEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("update sys_function set  ");
            strsql.Append("funcode=@funcode,funname=@funname,funseq=@funseq,funicon=@funicon,remark=@remark");
            strsql.Append(" where id=@id ");
            return SqlHelper.Execute(strsql.ToString(), model);
        }


        #endregion 实现接口方法

      
    }
}
