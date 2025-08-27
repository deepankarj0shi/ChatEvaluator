using ChatEvaluator.SharedObject.Models;
using ChatEvaluator.SharedObject;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Reflection;
using System.ComponentModel.DataAnnotations;


namespace ChatEvaluator.Dal.SqlServer
{
    public static class SqlServerBase
    {
        public static async Task<T> GetData<T>(string sql, object[] parms = null) where T: new()
        {
            try
            {
                using var connection = new SqlConnection();
                connection.ConnectionString = SharedObject.SharedObject.ConnectionString;
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.Connection = connection;
                command.SetParameters(parms);
                connection.Open();
                using var reader = await command.ExecuteReaderAsync();
                if (!await reader.ReadAsync())
                    return default;
                var result = new T();
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    var prop = properties.FirstOrDefault(p => string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));

                    if (prop != null && !reader.IsDBNull(i))
                    {
                        object value = reader.GetValue(i);
                        prop.SetValue(result, Convert.ChangeType(value, prop.PropertyType));
                    }
                }

                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task<List<T>> GetDataList<T>(string sql, object[] parms = null) where T : new()
        {
            try
            {
                using var connection = new SqlConnection();
                connection.ConnectionString = SharedObject.SharedObject.ConnectionString;
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.Connection = connection;
                command.SetParameters(parms);
                connection.Open();
                using var reader = await command.ExecuteReaderAsync();
                var resultList = new List<T>();
                while (await reader.ReadAsync())
                {
                    var result = new T();
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        var prop = properties.FirstOrDefault(p => string.Equals(p.Name, columnName, StringComparison.OrdinalIgnoreCase));

                        if (prop != null && !reader.IsDBNull(i))
                        {
                            object value = reader.GetValue(i);
                            prop.SetValue(result, Convert.ChangeType(value, prop.PropertyType));
                        }
                    }
                    resultList.Add(result);
                }
                return resultList;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static async Task<int> SaveData(string sql, object[] parms = null)
        {
            try
            {
                using var connection = new SqlConnection();
                connection.ConnectionString = SharedObject.SharedObject.ConnectionString;
                using var command = connection.CreateCommand();
                command.CommandText = sql.AppendIdentitySelect();
                command.Connection = connection;
                command.SetParameters(parms);
                connection.Open();
                return Convert.ToInt32(await command.ExecuteScalarAsync());

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static void SetParameters(this SqlCommand command, object[] parms)
        {
            if (parms != null && parms.Length > 0)
            {
                // NOTE: Processes a name/value pair at each iteration
                for (int i = 0; i < parms.Length; i += 2)
                {
                    string name = parms[i].ToString();
                    // If null, set to DbNull
                    object value = parms[i + 1] ?? DBNull.Value;

                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = name;
                    dbParameter.Value = value;

                    command.Parameters.Add(dbParameter);
                }
            }
        }
        private static string AppendIdentitySelect(this string sql)
        {
            return sql + ";SELECT SCOPE_IDENTITY()";
        }
    }
}
