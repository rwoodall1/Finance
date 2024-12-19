using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Utilities;
namespace SqlClient
{
    public class SQLQueryProperties
    {

        private static string _ConnectionString = ApplicationConfig.DefaultConnectionString;
        public SQLQueryProperties()
        {
            ConnectionString = _ConnectionString;
            Timeout = 20;
            CommandParameters = new List<SqlParameter>();
            CommandType = CommandType.Text;
            ReturnList = false;
            ReturnBindingListView = false;
            IdentityColumn = null;
        }

        public string ConnectionString { get; set; }
        public string CommandText { get; set; }
        public int Timeout { get; set; }
        public CommandType CommandType { get; set; }
        public List<SqlParameter> CommandParameters { get; set; }
        public SqlParameter[] CommandParametersCollection { get; set; }
        public bool ReturnList { get; set; }
        public bool ReturnBindingListView { get; set; }
        public string IdentityColumn { get; set; }
        public bool ReturnSqlIdentityId { get; set; }
        public DataTable BulkCopyDataTable { get; set; }
    }

    public class SQLParam
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public bool isDateTime { get; set; } = true;
    }
}
