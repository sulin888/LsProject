using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.Cores
{
    public sealed class DbConnectionFactory
    {
        private ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings["DbConnection"];

        private static DbConnectionFactory _instance;
        private DbConnectionFactory()
        { }

        public static DbConnectionFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DbConnectionFactory();
                }
                return _instance;
            }
        }
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetOpenConnection()
        {
            IDbConnection con = null;
            string dbType = connectionStringSettings.ProviderName.Trim();
            string connectionString = connectionStringSettings.ConnectionString;

            switch (dbType)
            {
                case "MySql":
                    con = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                    break;
                case "SqlServer":
                    con = new System.Data.SqlClient.SqlConnection(connectionString);
                    break;
                default:
                    break;
            }

            if (con == null)
            {
                throw new Exception("数据库连接配置不正确。");
            }

            return con;

        }
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetOpenConnection(ConnectionStringSettings conString = null)
        {
            IDbConnection con = null;
            string dbType = connectionStringSettings.ProviderName.Trim();
            string connectionString = connectionStringSettings.ConnectionString;
            if (conString != null)
            {
                dbType = conString.ProviderName.Trim();
                connectionString = conString.ConnectionString;
            }
            switch (dbType)
            {
                case "MySql":
                    con = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                    break;
                case "SqlServer":
                    con = new System.Data.SqlClient.SqlConnection(connectionString);
                    break;
                default:
                    break;
            }

            if (con == null)
            {
                throw new Exception("数据库连接配置不正确。");
            }

            return con;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetClosedConnection()
        {
            IDbConnection con = null;
            string dbType = connectionStringSettings.ProviderName.Trim();
            string connectionString = connectionStringSettings.ConnectionString;

            switch (dbType)
            {
                case "MySql":
                    con = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                    break;
                case "SqlServer":
                    con = new System.Data.SqlClient.SqlConnection(connectionString);
                    break;
                default:
                    break;
            }

            if (con == null)
            {
                throw new Exception("数据库连接配置不正确。");
            }

            return con;

        }
    }
}
