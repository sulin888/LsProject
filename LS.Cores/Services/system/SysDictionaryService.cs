using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LS.Entitys;
namespace LS.Cores
{
    /// <summary>
    /// 提供字典管理服务
    /// </summary>
    [Serializable]
    public class SysDictionaryService : ISysDictionary
    {
        #region 实现接口方法
        public int Delete(string id)
        {
            string sql = "delete from  sys_dictionary where id=@Id";
            return SqlHelper.Execute(sql, new { Id = id });
        }
        public SysDictionaryEntity GetById(int id)
        {
            string sql = "select * from  sys_dictionary where id=@Id";
            return SqlHelper.Query<SysDictionaryEntity>(sql, new { Id = id }).FirstOrDefault();
        }
        public PageDataView<SysDictionaryEntity> GetPageData(string strwhere,int currentPage=1,int pageSize=20)
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
            criteria.TableName = "sys_dictionary";
            criteria.PrimaryKey = "id";
            criteria.Sort = " dcode asc ";
            return SqlHelper.GetPageData<SysDictionaryEntity>(criteria);
        }
        public IList<SysDictionaryEntity> Query(string strwhere)
        {
            string sql = "select * From  sys_dictionary where isenable=1  ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += strwhere;
            }
            return SqlHelper.Query<SysDictionaryEntity>(sql).ToList();
        }
        public int Insert(SysDictionaryEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("insert into sys_dictionary(dcode,parentcode,dname,dseq,isenable,dictype,remark,createby,createdate) ");
            strsql.Append(" values(@dcode,@parentcode,@dname,@dseq,@isenable,@dictype,@remark,@createby,@createdate)");
            return SqlHelper.Execute(strsql.ToString(), model);
        }
        public int Update(SysDictionaryEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("update sys_dictionary set  ");
            strsql.Append("dcode=@dcode,parentcode=@parentcode,dname=@dname,dseq=@dseq,isenable=@isenable,dictype=@dictype,remark=@remark,createby=@createby,createdate=@createdate");
            strsql.Append(" where id=@id ");
            return SqlHelper.Execute(strsql.ToString(), model);
        }

       
        #endregion 实现接口方法
    }
}
