using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Utilities;

namespace SQLClient {
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
            IdentityColumn = null;
        }

        public string ConnectionString { get; set; }
        public string CommandText { get; set; }
        public int Timeout { get; set; }
        public CommandType CommandType { get; set; }
        public List<SqlParameter> CommandParameters { get; set; }
        public SqlParameter[] CommandParametersCollection { get; set; }
        public bool ReturnList { get; set; }
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
