using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Cores
{
    public sealed class SqlHelper
    {
        private SqlHelper()
        { }

        /// <summary>
        /// 查询数据-集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<dynamic> Query(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, ConnectionStringSettings conString = null)
        {
            using (IDbConnection con = DbConnectionFactory.Instance.GetOpenConnection(conString))
            {
                con.Open();
                return con.Query(sql, param, transaction, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 查询数据-集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, ConnectionStringSettings conString = null)
        {
            using (IDbConnection con = DbConnectionFactory.Instance.GetOpenConnection(conString))
            {
                con.Open();
                return con.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 执行异步查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ConnectionStringSettings conString = null)
        {
            using (IDbConnection con = DbConnectionFactory.Instance.GetOpenConnection(conString))
            {
                con.Open();
                return con.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 执行异步查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> QueryAsync<T>(CommandDefinition commandDefinition, ConnectionStringSettings conString = null)
        {
            using (IDbConnection con = DbConnectionFactory.Instance.GetOpenConnection(conString))
            {
                con.Open();
                return con.QueryAsync<T>(commandDefinition);
            }
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="totalCount"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static IEnumerable<T> QueryMultiple<T>(string sql, out int totalCount, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, ConnectionStringSettings conString = null)
        {
            using (IDbConnection con = DbConnectionFactory.Instance.GetOpenConnection(conString))
            {
                con.Open();
                var multi = con.QueryMultiple(sql, param, transaction, commandTimeout, commandType);

                totalCount = int.Parse(multi.Read<long>().Single().ToString());
                return multi.Read<T>();
            }
        }

        /// <summary>
        /// 查询单个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static T QueryOne<T>(string sql, object param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null, ConnectionStringSettings conString = null) where T : class
        {
            var dataResult = Query<T>(sql, param, transaction, buffered, commandTimeout, commandType, conString);
            return dataResult != null && dataResult.Count() > 0 ? dataResult.ToList<T>()[0] : null;
        }

        /// <summary>
        /// 返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int Execute(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, ConnectionStringSettings conString = null)
        {

            using (IDbConnection con = DbConnectionFactory.Instance.GetOpenConnection(conString))
            {
                con.Open();
                return con.Execute(sql, param, transaction, commandTimeout, commandType);
            }
        }
        public static int ExecuteCon(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null, IDbConnection con=null)
        {

            return con.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(string sql, object param = null, ConnectionStringSettings conString = null)
        {
            using (IDbConnection con = DbConnectionFactory.Instance.GetOpenConnection(conString))
            {
                con.Open();
                return con.ExecuteScalar<T>(sql, param);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static T Execute<T>(string command, Dictionary<string, object> paras, ConnectionStringSettings conString = null)
        {
            using (IDbConnection con = DbConnectionFactory.Instance.GetOpenConnection(conString))
            {
                IDbCommand com = con.CreateCommand();
                com.CommandText = command;
                com.CommandType = CommandType.StoredProcedure;

                if (paras != null)
                {
                    foreach (var item in paras.Keys)
                    {
                        IDbDataParameter para = com.CreateParameter();
                        para.Value = paras[item];
                        para.ParameterName = item;
                        com.Parameters.Add(para);
                    }
                }

                con.Open();
                return (T)com.ExecuteScalar();
            }
        }

        /// <summary>
        /// 读取blob字段
        /// </summary>
        /// <returns></returns>
        public static byte[] ReadBlob(string command, object paras, ConnectionStringSettings conString = null)
        {
            return QueryOne<byte[]>(command, paras, null, false, null, CommandType.Text, conString);
        }

        /// <summary>
        /// 更新blob
        /// </summary>
        /// <param name="command"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool WriteBlob(string command, object param, ConnectionStringSettings conString = null)
        {
            int effactRows = Execute(command, param, null, null, CommandType.Text, conString);

            return effactRows > 0;
        }

        #region Dapper-Extensions
        public static PageDataView<T> GetPageData<T>(PageCriteria criteria, object param = null, ConnectionStringSettings conString = null)
        {
            using (IDbConnection conn = DbConnectionFactory.Instance.GetOpenConnection(conString))
            {
                var p = new DynamicParameters();
                string proName = "proc_GetPageData";
                p.Add("TableName", criteria.TableName);
                p.Add("PrimaryKey", criteria.PrimaryKey);
                p.Add("Fields", criteria.Fields);
                p.Add("Condition", criteria.Condition);
                p.Add("CurrentPage", criteria.CurrentPage);
                p.Add("PageSize", criteria.PageSize);
                p.Add("Sort", criteria.Sort);
                p.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                conn.Open();
                var pageData = new PageDataView<T>();
                pageData.Items = conn.Query<T>(proName, p, commandType: CommandType.StoredProcedure).ToList();
                conn.Close();
                pageData.TotalNum = p.Get<int>("RecordCount");

                pageData.TotalPageCount = Convert.ToInt32(Math.Ceiling(pageData.TotalNum * 1.0 / criteria.PageSize));
                pageData.CurrentPage = criteria.CurrentPage > pageData.TotalPageCount ? pageData.TotalPageCount : criteria.CurrentPage;
                return pageData;
            }
        }

        #endregion

        public static bool ExecuteTranSql(Func<IDbConnection, IDbTransaction, int> func, ConnectionStringSettings conString = null)
        {
            int r = 0;
            GetConnection(conString, conn =>
            {
                IDbTransaction trans = conn.BeginTransaction();
                r = func(conn, trans);
                if (r > 0)
                {
                    trans.Commit(); 
                }
                else
                {
                    trans.Rollback();
                }
            });

            return r > 0;
        }
        public static void GetConnection(ConnectionStringSettings conString, Action<IDbConnection> action)
        {
            using (var conn = DbConnectionFactory.Instance.GetOpenConnection(conString))
            {
                conn.Open();
                action(conn);
            }
        }
    }
}
