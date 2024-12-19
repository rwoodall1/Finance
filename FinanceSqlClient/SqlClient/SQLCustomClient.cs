using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Core;
namespace SqlClient
{
    public class SQLCustomClient
    {

        public SQLCustomClient()
        {
            Target = new SQLQueryProperties();
            IDBConnectionInternal = new SqlConnection(Target.ConnectionString);
        }

        public SQLQueryProperties Target { get; private set; }
        public IDbConnection IDBConnectionInternal { get; private set; }

        // Functions
        public ApiProcessingResult<object> Select<T>() { Target.ReturnList = false; return SQLCore.ExecuteReader<T>(Target); }
        public ApiProcessingResult<string> SelectSingleColumn() { return SQLCore.ExecuteScalar(Target); }
        public ApiProcessingResult<object> SelectManyView<T>() { Target.ReturnList = true; Target.ReturnBindingListView = true; return SQLCore.ExecuteReader<T>(Target); }
        public ApiProcessingResult<object> SelectMany<T>() { Target.ReturnList = true;return SQLCore.ExecuteReader<T>(Target); }
        public ApiProcessingResult<object> SelectMany() { Target.ReturnList = false; return SQLCore.ExecuteReader(Target); }
        public ApiProcessingResult<int> Update() { return SQLCore.ExecuteNonQuery(Target); }
        public ApiProcessingResult<int> Delete() { return SQLCore.ExecuteNonQuery(Target); }
        public ApiProcessingResult<object> StoredProcedure() { Target.CommandType = System.Data.CommandType.StoredProcedure; return SQLCore.ExecuteReader(Target); }
        public ApiProcessingResult<string> BulkCopy() { return SQLCore.BulkCopy(Target); }
        public ApiProcessingResult<string> Insert() { return SQLCore.ExecuteScalar(Target); }
        public ApiProcessingResult<string> ExecuteScalar() { Target.ReturnSqlIdentityId = false; return SQLCore.ExecuteScalar(Target); }

        //Dapper
        public IDbConnection Dapper() { return IDBConnectionInternal; }

        //Setters
        public SQLCustomClient ConnectionString(string ConnectionString) { Target.ConnectionString = ConnectionString; return this; }
        public SQLCustomClient Timeout(int Timeout) { Target.Timeout = Timeout; return this; }
        public SQLCustomClient CommandType(CommandType CommandType) { Target.CommandType = CommandType; return this; }
        public SQLCustomClient CommandText(string CommandText) { Target.CommandText = CommandText; return this; }
        //public string ParametersToJSON()
        //{
        //    var removeParamsList = new List<string> { "PASSPHRASE" };
        //    var maskParamsList = new List<string> { "SSN", "DOB", "DATEOFBIRTH", "QBSSN", "QBDATEOFBIRTH", "QBDOB" };

        //    dynamic myDym = new System.Dynamic.ExpandoObject();

        //    foreach (SqlParameter param in Target.CommandParameters)
        //    {
        //        var myParamName = param.ParameterName.Replace("@", string.Empty);
        //        if (removeParamsList.Contains(myParamName.ToUpper()))
        //        {
        //            //Do Nothing - This will not be logged
        //        }
        //        else if (maskParamsList.Contains(myParamName.ToUpper()) && param.Value != null)
        //        {
        //            var myMask = "";
        //            ((IDictionary<String, Object>)myDym).Add(myParamName, myMask.PadLeft(param.Value.ToString().Length, '*'));
        //        }
        //        else
        //        {
        //            ((IDictionary<String, Object>)myDym).Add(myParamName, param.Value);
        //        }
        //    }

        //    return JsonConvert.SerializeObject(myDym);
        //}
        public SQLCustomClient AddParameter(string name, object value, bool nvarcharmax = false)
        {
            if (value == null)
            {
                Target.CommandParameters.Add(new SqlParameter(name, DBNull.Value));
            }
            else
            {
                if (nvarcharmax)
                {
                    Target.CommandParameters.Add(new SqlParameter(name, SqlDbType.NVarChar, -1) { Value = value });
                }
                else
                {
                    Target.CommandParameters.Add(new SqlParameter(name, value));
                }

            }
            return this;
        }
        public SQLCustomClient AddParameter(string name, object value, SqlDbType dbtype)
        {
            if (value == null)
            {
                Target.CommandParameters.Add(new SqlParameter(name, DBNull.Value));
            }
            else
            {
                Target.CommandParameters.Add(new SqlParameter(name, dbtype) { Value = value });
            }
            return this;
        }
        public SQLCustomClient AddParameter(string name, DateTime? value, bool IsDateTime = true)
        {
            if (value == null)
            {
                Target.CommandParameters.Add(new SqlParameter(name, DBNull.Value));
            }
            else if (IsDateTime)
            {
                Target.CommandParameters.Add(new SqlParameter(name, SqlDbType.DateTime) { Value = value });
            }
            else
            {
                Target.CommandParameters.Add(new SqlParameter(name, SqlDbType.Date) { Value = value });
            }

            return this;
        }
        public SQLCustomClient AddParameter(string name, DateTime value, bool IsDateTime = true)
        {
            if (IsDateTime)
            {
                Target.CommandParameters.Add(new SqlParameter(name, SqlDbType.DateTime) { Value = value });
            }
            else
            {
                Target.CommandParameters.Add(new SqlParameter(name, SqlDbType.Date) { Value = value });
            }

            return this;
        }
        public SQLCustomClient AddParameter(string name, ICollection<string> list)
        {
            DataTable LocList = new DataTable();
            LocList.Columns.Add("Item", typeof(String));
            if (list.Count() > 0)
            {
                foreach (string Id in list)
                {
                    LocList.Rows.Add(Id);
                }
            }
            else
            {
                LocList.Rows.Add("");
            }

            Target.CommandParameters.Add(new SqlParameter(name, SqlDbType.Structured) { TypeName = "ItemList", Value = LocList });
            return this;
        }
        public SQLCustomClient ClearParameters()
        {
            Target.CommandParameters.Clear();
            return this;

        }
        public SQLCustomClient ReturnList(bool returnList) { Target.ReturnList = returnList; return this; }
        public SQLCustomClient ReturnListView(bool ReturnBindingListView) { Target.ReturnBindingListView = ReturnBindingListView; return this; }
        public SQLCustomClient ReturnSqlIdentityId(bool val) { Target.ReturnSqlIdentityId = val; return this; }
        public SQLCustomClient IdentityColumn(string val) { Target.IdentityColumn = val; return this; }
        public SQLCustomClient BulkCopyDataTable(DataTable val) { Target.BulkCopyDataTable = val; return this; }
    }
}
