using Core;
////using Exceptionless;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using Utilities;

namespace SQLClient {
    public static class SQLCore
    {
        public static ApiProcessingResult<int> ExecuteNonQuery(SQLQueryProperties _sqlProperties)
        {
            var result = new ApiProcessingResult<int>();

            using (var connection = new SqlConnection(_sqlProperties.ConnectionString))
            {
                using (var command = new SqlCommand(_sqlProperties.CommandText, connection))
                {
                    try
                    {
                        command.CommandType = _sqlProperties.CommandType;
                        command.Parameters.Clear();
                        if (_sqlProperties.CommandParametersCollection != null && _sqlProperties.CommandParametersCollection.Count() > 0)
                        {
                            command.Parameters.AddRange(_sqlProperties.CommandParametersCollection);
                        }
                        else if (_sqlProperties.CommandParameters != null)
                        {
                            command.Parameters.AddRange(_sqlProperties.CommandParameters.ToArray());
                        }
                        command.CommandTimeout = _sqlProperties.Timeout;
                        connection.Open();

                        var sqlResult = command.ExecuteNonQuery();
                        result.Data = sqlResult;
                    }
                    catch (Exception ex)
                    {
                        //ex.ToExceptionless()
                        // .SetMessage("Error running ExecuteNonQuery.")
                        // .AddTags("SqlQueryWrapper")
                        // .AddObject(_sqlProperties.CommandText, "Query")
                        // .AddObject(Utils.CommandParametersToObj(_sqlProperties.CommandParameters.ToArray()), "Query Parameters")
                        // .MarkAsCritical()
                        // .Submit();
                        result.IsError = true;
                        result.Errors.Add(new ApiProcessingError("Error running ExecuteNonQueryAsync. Ex: " + ex.Message.ToString(), "Error running ExecuteNonQueryAsync", "EXNQSQL"));
                    }
                    finally
                    {
                        connection.Close();
                        connection.Dispose();
                    }

                    return result;
                }
            }
        }

        public static ApiProcessingResult<object> ExecuteReader<T>(SQLQueryProperties _sqlProperties)
        {
            var processingResult = new ApiProcessingResult<object>();

            using (var connection = new SqlConnection(_sqlProperties.ConnectionString))
            {
                using (var command = new SqlCommand(_sqlProperties.CommandText, connection))
                {
                    try
                    {
                        var dt = new DataTable();
                        command.CommandType = _sqlProperties.CommandType;
                        command.Parameters.Clear();
                        if (_sqlProperties.CommandParametersCollection != null && _sqlProperties.CommandParametersCollection.Count() > 0)
                        {
                            command.Parameters.AddRange(_sqlProperties.CommandParametersCollection);
                        }
                        else if (_sqlProperties.CommandParameters != null)
                        {
                            command.Parameters.AddRange(_sqlProperties.CommandParameters.ToArray());
                        }
                        command.CommandTimeout = _sqlProperties.Timeout;
                        connection.Open();

                        using (SqlDataAdapter a = new SqlDataAdapter(command))
                        {
                            a.Fill(dt);
                        }

                        if (dt.Rows.Count < 1)
                        {
                            processingResult.Data = null;
                            return processingResult;
                        }

                        try
                        {
                            IList<T> ValsList;
                            T ValsObject;

                            if (_sqlProperties.ReturnList)
                            {
                                ValsList = CollectionHelper.ConvertTo<T>(dt);
                                processingResult.Data = ValsList;
                            }
                            else
                            {
                                ValsObject = CollectionHelper.ConvertToObject<T>(dt);
                                processingResult.Data = ValsObject;
                            }
                        }
                        catch (Exception ex)
                        {
                            //ex.ToExceptionless()
                            //   .SetMessage("Collection Helper Error")
                            //   .AddTags("Collection Helper")
                            //   .MarkAsCritical()
                            //   .Submit();
                            processingResult.IsError = true;
                            processingResult.Errors.Add(new ApiProcessingError("Error retrieving data from ExecuteReader:" + ex.Message, "Error retrieving data from ExecuteReader: " + ex.Message, "EXROBJ"));
                        }

                        return processingResult;
                    }
                    catch (Exception ex)
                    {
                        //ex.ToExceptionless()
                        //   .SetMessage("Error running ExecuteReaderAsync(object).")
                        //   .AddTags("Sql Query Wrapper")
                        //   .AddObject(_sqlProperties.CommandText, "Query")
                        //   .AddObject(Utils.CommandParametersToObj(_sqlProperties.CommandParameters.ToArray()), "Query Parameters")
                        //   .MarkAsCritical()
                        //   .Submit();
                        processingResult.IsError = true; ;
                        processingResult.Errors.Add(new ApiProcessingError("Error running ExecuteReaderAsync. Ex: " + ex.Message.ToString(), "Error running ExecuteReaderAsync.", "EXRERR"));
                        return processingResult;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public static ApiProcessingResult<object> ExecuteReader(SQLQueryProperties _sqlProperties)
        {
            var processingResult = new ApiProcessingResult<object>();
            //returns table
            using (var connection = new SqlConnection(_sqlProperties.ConnectionString))
            {
                using (var command = new SqlCommand(_sqlProperties.CommandText, connection))
                {
                    try
                    {
                        var dt = new DataTable();

                        command.CommandType = _sqlProperties.CommandType;
                        command.Parameters.Clear();
                        if (_sqlProperties.CommandParametersCollection != null && _sqlProperties.CommandParametersCollection.Count() > 0)
                        {
                            command.Parameters.AddRange(_sqlProperties.CommandParametersCollection);
                        }
                        else if (_sqlProperties.CommandParameters != null)
                        {
                            command.Parameters.AddRange(_sqlProperties.CommandParameters.ToArray());
                        }
                        command.CommandTimeout = _sqlProperties.Timeout;
                        connection.Open();

                        using (SqlDataAdapter a = new SqlDataAdapter(command))
                        {
                            a.Fill(dt);
                        }

                        if (dt.Rows.Count < 1)
                        {
                            processingResult.Data = null;
                        }
                        else
                        {
                           
                            processingResult.Data = dt;
                        }

                        return processingResult;
                    }
                    catch (Exception ex)
                    {
                        //ex.ToExceptionless()
                        //   .SetMessage("Error running ExecuteReaderAsync(object).")
                        //   .AddTags("Sql Query Wrapper")
                        //   .AddObject(_sqlProperties.CommandText, "Query")
                        //   .AddObject(Utils.CommandParametersToObj(_sqlProperties.CommandParameters.ToArray()), "Query Parameters")
                        //   .MarkAsCritical()
                        //   .Submit();
                        processingResult.IsError = true;
                        processingResult.Errors.Add(new ApiProcessingError("Error running ExecuteReaderAsync. Ex: " + ex.Message.ToString(), "Error running ExecuteReaderAsync. Ex: ", "ERREXR"));
                        return processingResult;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        public static ApiProcessingResult<string> ExecuteScalar(SQLQueryProperties _sqlProperties)
        {
            var result = new ApiProcessingResult<string>();
            if (!string.IsNullOrEmpty(_sqlProperties.IdentityColumn))
            {
                _sqlProperties.CommandText = _sqlProperties.CommandText.Replace(") VALUES (", ") OUTPUT INSERTED.[" + _sqlProperties.IdentityColumn + "] VALUES (");
                _sqlProperties.CommandText = _sqlProperties.CommandText.Replace(")VALUES(", ") OUTPUT INSERTED.[" + _sqlProperties.IdentityColumn + "] VALUES (");
                _sqlProperties.CommandText = _sqlProperties.CommandText.Replace(")VALUES (", ") OUTPUT INSERTED.[" + _sqlProperties.IdentityColumn + "] VALUES (");
                _sqlProperties.CommandText = _sqlProperties.CommandText.Replace(") VALUES(", ") OUTPUT INSERTED.[" + _sqlProperties.IdentityColumn + "] VALUES (");
            }
            else
            {
                _sqlProperties.CommandText += ";SELECT CAST(scope_identity() AS varchar)";
            }

            using (var connection = new SqlConnection(_sqlProperties.ConnectionString))
            {
                using (var command = new SqlCommand(_sqlProperties.CommandText, connection))
                {
                    try
                    {
                        command.CommandType = _sqlProperties.CommandType;
                        command.Parameters.Clear();
                        command.CommandTimeout = _sqlProperties.Timeout;
                        if (_sqlProperties.CommandParametersCollection != null && _sqlProperties.CommandParametersCollection.Count() > 0)
                        {
                            command.Parameters.AddRange(_sqlProperties.CommandParametersCollection);
                        }
                        else if (_sqlProperties.CommandParameters != null)
                        {
                            command.Parameters.AddRange(_sqlProperties.CommandParameters.ToArray());
                        }
                        connection.Open();

                        var sqlResult = command.ExecuteScalar();
                        if (sqlResult == null)
                        {
                            sqlResult = "";
                        }
                        result.Data = sqlResult.ToString();
                    }
                    catch (Exception ex)
                    {
                        //ex.ToExceptionless()
                        //  .SetMessage("Error running ExecuteScalarAsync.")
                        //  .AddTags("SqlQueryWrapper")
                        //  .AddObject(_sqlProperties.CommandText, "Query")
                        //  .AddObject(Utils.CommandParametersToObj(_sqlProperties.CommandParameters.ToArray()), "Query Parameters")
                        //  .MarkAsCritical()
                        //  .Submit();
                        result.IsError = true;
                        result.Errors.Add(new ApiProcessingError("Error processing ExecuteScalarAsync Ex: " + ex.Message.ToString(), "Error processing ExecuteScalarAsync", "EXSERR"));
                    }
                    finally
                    {
                        connection.Close();
                    }

                    return result;
                }
            }
        }
        public static ApiProcessingResult<string> BulkCopy(SQLQueryProperties _sqlProperties)
        {
            var result = new ApiProcessingResult<string>();

            using (var conn = new SqlConnection(_sqlProperties.ConnectionString))
            {
                var sqlBulkCopy = new SqlBulkCopy(conn);
                sqlBulkCopy.DestinationTableName = _sqlProperties.CommandText;
                conn.Open();

                var schema = conn.GetSchema("Columns", new[] { null, null, _sqlProperties.CommandText, null });

                foreach (DataColumn sourceColumn in _sqlProperties.BulkCopyDataTable.Columns)
                {
                    foreach (DataRow row in schema.Rows)
                    {
                        if (string.Equals(sourceColumn.ColumnName, (string)row["COLUMN_NAME"], StringComparison.OrdinalIgnoreCase))
                        {
                            sqlBulkCopy.ColumnMappings.Add(sourceColumn.ColumnName, (string)row["COLUMN_NAME"]);
                            break;
                        }
                    }
                }

                sqlBulkCopy.WriteToServer(_sqlProperties.BulkCopyDataTable);
                conn.Close();
            }
            return result;
        }
    }
}
