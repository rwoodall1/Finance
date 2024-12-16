////using Exceptionless;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Utilities
{
    public class CollectionHelper
    {
        private CollectionHelper()
        {
        }

        public static DataTable ConvertTo<T>(IList<T> list)
        {

            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }
        //2
        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static T ConvertToObject<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list[0];
        }
        //1
        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }
        //1
        public static T ConvertToObject<T>(DataTable table)
        {
            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertToObject<T>(rows);
        }
        //3
        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        //object value = (row[column.ColumnName] == DBNull.Value ? null : Convert.ChangeType(row[column.ColumnName], Nullable.GetUnderlyingType(prop.PropertyType)));

                        //object value = (row[column.ColumnName] == DBNull.Value ? null : row[column.ColumnName]);
                        if (prop != null)
                        {
                            ParsePrimitive(prop, obj, row[column.ColumnName]);
                            //prop.SetValue(obj, value, null);
                        }

                    }
                    catch (Exception ex)
                    {
                        //ex.ToExceptionless()
                        //.SetMessage("Failed to create item.")
                        //.AddTags("Collection Helper")
                        //.MarkAsCritical()
                        //.AddObject(row, "Data (Row)")
                        //.AddObject(obj, "Object (Row)")
                        //.AddObject(column, "Column")
                        //.AddObject(prop, "Column Properties")
                        //.AddObject(row[column.ColumnName], "Value")
                        //.Submit();
                        //throw;
                    }
                }
            }

            return obj;
        }

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }

        private static void ParsePrimitive(PropertyInfo prop, object entity, object value)
        {
            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(entity, value.ToString().Trim(), null);
            }
            else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(int?))
            {
                if (value == null || value == DBNull.Value)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, int.Parse(value.ToString()), null);
                }
            }
            else if (prop.PropertyType == typeof(Int32) || prop.PropertyType == typeof(Int32?))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    prop.SetValue(entity, Int32.Parse(value.ToString()), null);
                }
            }
            else if (prop.PropertyType == typeof(Int64) || prop.PropertyType == typeof(Int64?))
            {
                if (value == null)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    try
                    {
                        prop.SetValue(entity, Int64.Parse(value.ToString()), null);
                    }
                    catch
                    {
                        prop.SetValue(entity, null, null);
                    }
                }
            }
            else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(Nullable<DateTime>))
            {
                DateTime date;
                bool isValid = DateTime.TryParse(value.ToString(), out date);
                if (isValid)
                {
                    prop.SetValue(entity, date, null);
                }
                else
                {
                    //Making an assumption here about the format of dates in the source data.
                    isValid = DateTime.TryParseExact(value.ToString(), "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.AssumeUniversal, out date);
                    if (isValid)
                    {
                        prop.SetValue(entity, date, null);
                    }
                }
            }
            else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
            {
                if (value == DBNull.Value)
                {
                    prop.SetValue(entity, null, null);
                }
                else
                {
                    try
                    {
                        prop.SetValue(entity, decimal.Parse(value.ToString()), null);
                    }
                    catch
                    {
                        prop.SetValue(entity, null, null);
                    }
                }


            }
            else if (prop.PropertyType == typeof(float))
            {
                prop.SetValue(entity, float.Parse(value.ToString()), null);
            }
            else if (prop.PropertyType == typeof(char))
            {
                prop.SetValue(entity, char.Parse(value.ToString()), null);
            }
            else if (prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(bool?))
            {
                if (value.ToString() == "1")
                {
                    prop.SetValue(entity, true, null);
                }
                else if (value.ToString() == "0")
                {
                    prop.SetValue(entity, false, null);
                }
                else
                {
                    bool boolValue;

                    bool isValid = bool.TryParse(value.ToString(), out boolValue);
                    if (isValid)
                    {
                        prop.SetValue(entity, bool.Parse(value.ToString()), null);
                    }
                }
            }
            else if (prop.PropertyType == typeof(long) || prop.PropertyType == typeof(long?))
            {
                long number;
                bool isValid = long.TryParse(value.ToString(), out number);
                if (isValid)
                {
                    prop.SetValue(entity, long.Parse(value.ToString()), null);
                }
            }
            else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(double?))
            {
                double number;
                bool isValid = double.TryParse(value.ToString(), out number);
                if (isValid)
                {
                    prop.SetValue(entity, double.Parse(value.ToString()), null);
                }
            }
            else if (prop.PropertyType == typeof(Guid) || prop.PropertyType == typeof(Guid?))
            {
                Guid guid;
                bool isValid = Guid.TryParse(value.ToString(), out guid);
                if (isValid)
                {
                    prop.SetValue(entity, Guid.Parse(value.ToString()), null);
                }
            }
            else
            {

            }
        }
    }
}
