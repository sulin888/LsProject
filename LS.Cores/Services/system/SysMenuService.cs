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
    /// 提供菜单管理服务
    /// </summary>
    [Serializable]
    public class SysMenuService : ISysMenu
    {
        #region 实现接口方法
        public int Delete(string id)
        {
            var model= GetById(Convert.ToInt32(id));
            bool resulte = SqlHelper.ExecuteTranSql((con, tran) =>
            {
                int r = 0;
                try
                {
                    string sql = "delete from  sys_menu where id=@Id";
                    r = SqlHelper.ExecuteCon(sql.ToString(), new { Id = id }, tran, null, CommandType.Text, con);

                    string sqlfun = "delete from  sys_menufunction where menucode=@Menucode";
                    r = SqlHelper.ExecuteCon(sqlfun.ToString(), new { Menucode = model.Menucode }, tran, null, CommandType.Text, con);

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
        public SysMenuEntity GetById(int id)
        {
            string sql = "select * from  sys_menu where id=@Id";
            return SqlHelper.Query<SysMenuEntity>(sql, new { Id = id }).FirstOrDefault();
        }
        public PageDataView<SysMenuEntity> GetPageData(string strwhere, int currentPage = 1, int pageSize = 20)
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
            criteria.TableName = "sys_menu";
            criteria.PrimaryKey = "id";
            return SqlHelper.GetPageData<SysMenuEntity>(criteria);
        }
        public IList<SysMenuEntity> Query(string strwhere)
        {
            string sql = "select * From  sys_menu where isvisible=0 and isenable=1  ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += strwhere;
            }
            return SqlHelper.Query<SysMenuEntity>(sql).ToList();
        }
        public int Insert(SysMenuEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("insert into sys_menu(menucode,parentcode,menuname,menuurl,menuicon,menuseq,isvisible,isenable,menulevel,createby,createdate,remark) ");
            strsql.Append(" values(@menucode,@parentcode,@menuname,@menuurl,@menuicon,@menuseq,@isvisible,@isenable,@menulevel,@createby,@createdate,@remark)");
            return SqlHelper.Execute(strsql.ToString(), model);
        }
        public int Update(SysMenuEntity model)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("update sys_menu set  ");
            strsql.Append("menucode=@menucode,parentcode=@parentcode,menuname=@menuname,menuurl=@menuurl,menuicon=@menuicon,menuseq=@menuseq,isvisible=@isvisible,isenable=@isenable,menulevel=@menulevel,createby=@createby,createdate=@createdate,remark=@remark");
            strsql.Append(" where id=@id ");
            return SqlHelper.Execute(strsql.ToString(), model);
        }


        #endregion 实现接口方法

        public IList<SysMenuFunctionEntity> GetMenuFunction(string strwhere)
        {
            string sql = "select * From  sys_menufunction where 1=1 ";
            if (!string.IsNullOrEmpty(strwhere))
            {
                sql += strwhere;
            }
            return SqlHelper.Query<SysMenuFunctionEntity>(sql).ToList();
        }

        public int InsertMenuFun(SysMenuEntity model, List<SysMenuFunctionEntity> listModel)
        {
            bool resulte = SqlHelper.ExecuteTranSql((con, tran) =>
            {
                int r = 0;
                try
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("insert into sys_menu(menucode,parentcode,menuname,menuurl,menuicon,menuseq,isvisible,isenable,menulevel,createby,createdate,remark) ");
                    strsql.Append(" values(@menucode,@parentcode,@menuname,@menuurl,@menuicon,@menuseq,@isvisible,@isenable,@menulevel,@createby,@createdate,@remark)");
                  
                    r = SqlHelper.ExecuteCon(strsql.ToString(), model,tran,null, CommandType.Text,con);
                 
                    if(listModel!=null && listModel.Count > 0)
                    {
                        StringBuilder strsqlfun = new StringBuilder();
                        strsqlfun.Append("insert into sys_menufunction(menucode,funcode) ");
                        strsqlfun.Append(" values(@menucode,@funcode)");
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

        public int UpdateMenuFun(SysMenuEntity model, List<SysMenuFunctionEntity> listModel)
        {
            bool resulte = SqlHelper.ExecuteTranSql((con, tran) =>
            {
                int r = 0;
                try
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.Append("update sys_menu set  ");
                    strsql.Append("menucode=@menucode,parentcode=@parentcode,menuname=@menuname,menuurl=@menuurl,menuicon=@menuicon,menuseq=@menuseq,isvisible=@isvisible,isenable=@isenable,menulevel=@menulevel,createby=@createby,createdate=@createdate,remark=@remark");
                    strsql.Append(" where id=@id ");
                    r = SqlHelper.ExecuteCon(strsql.ToString(), model, tran, null, CommandType.Text, con);

                    if (listModel != null && listModel.Count > 0)
                    {
                        string sqlfun = "delete from  sys_menufunction where menucode=@Menucode";
                        r = SqlHelper.ExecuteCon(sqlfun.ToString(), new { Menucode = model.Menucode }, tran, null, CommandType.Text, con);
                        StringBuilder strsqlfun = new StringBuilder();
                        strsqlfun.Append("insert into sys_menufunction(menucode,funcode) ");
                        strsqlfun.Append(" values(@menucode,@funcode)");
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

        public Hashtable GetMaxPartMenu(string menuCode)
        {
            Hashtable hs = new Hashtable();
            string sql = "select isnull(MAX(MenuCode),'') MenuCode ,isnull(max(MenuSeq),0) MenuSeq  From sys_menu ";
            if (string.IsNullOrEmpty(menuCode))
            {
                sql += " where menulevel=0 ";
            }
            else
            {
                sql += " where ParentCode='" + menuCode + "' ";
            }
            var model = SqlHelper.Query(sql).FirstOrDefault();
            foreach (var it in model)
            {
                if (it.Key == "MenuCode")
                {
                    hs.Add("MenuCode", it.Value);
                }
                if (it.Key == "MenuSeq")
                {
                    hs.Add("MenuSeq", it.Value);
                }

            }
            return hs;
        }

        public List<SysMenuEntity> GetUserPartMenuAdmin(string menuCode,int mlevel)
        {

            string sql = " select * From sys_menu where   isvisible=0 and isenable=1 ";
            if (mlevel==0)
            {
                sql += " and menulevel=0 ";
                return SqlHelper.Query<SysMenuEntity>(sql).ToList();
            }
            else
            {
                sql += " and  ParentCode like @mcode  ";
                return SqlHelper.Query<SysMenuEntity>(sql, new { mcode = menuCode + "%" }).ToList();
            }
           
        }
        public List<SysMenuEntity> GetUserPartMenu(string userCode, int mlevel)
        {

            return SqlHelper.Query<SysMenuEntity>("p_getUserMenu", new { UserCode = userCode }, commandType: CommandType.StoredProcedure).ToList();
          
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> GetMenuList(string strwhere)
        {
            StringBuilder strsql = new StringBuilder();
            strsql.Append("select A.*,isnull((select top 1 id from sys_menu where menucode=A.parentcode),0) pid,");
            strsql.Append("dbo.f_menufunction(A.menucode) as funcode ");
            strsql.Append("from sys_menu A ");
            if (!string.IsNullOrEmpty(strwhere))
            {
                strsql.Append(" where 1=1 "+ strwhere);
            }
            strsql.Append("order by menucode,parentcode,menuseq");
            return SqlHelper.Query<dynamic>(strsql.ToString());
        }
    }
}
