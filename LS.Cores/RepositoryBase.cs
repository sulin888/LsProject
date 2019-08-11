using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Cores
{
    public interface RepositoryBase<T> where T : class, new()
    {
        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Insert(T model);
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(T model);
        /// <summary>
        /// 根据编号获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(int id);
    
        ///<summary>
        ///删除对象 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete(string id);
        /// <summary>
        /// 根据条件返回数据
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        IList<T> Query(string strwhere);
        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        PageDataView<T> GetPageData(string strwhere, int currentPage = 1, int pageSize = 20);
    }
}
