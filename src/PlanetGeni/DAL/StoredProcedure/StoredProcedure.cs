using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections;


namespace DAO
{
    public class StoredProcedure
    {
        # region Properties
        private string ConnectionStringKey;
        private string connectionString { get; set; }
        private bool alreadyLoadedconnectionString { get; set; }
        private readonly string ParmLastId = "@parmLastId";
        protected string GetConnectionString
        {
            get
            {
                if (!alreadyLoadedconnectionString)
                    LoadConnectionString();

                return connectionString;
            }
            set
            {
                connectionString = value;
                alreadyLoadedconnectionString = true;
            }
        }
        private void LoadConnectionString()
        {
            if (connectionString == null)
            {
                connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey].ConnectionString;
            }
            alreadyLoadedconnectionString = true;
        }

        #endregion

        public StoredProcedure()
        {
            ConnectionStringKey = "MySQLConnectionString";
        }
        public StoredProcedure(string connectionStringKey)
        {
            ConnectionStringKey = connectionStringKey;
        }
        public T GetSqlDataSignleRow<T>(string sql, Dictionary<string, object> parmlist) where T : new()
        {
            var properties = typeof(T).GetProperties();

            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, object> kvp in parmlist)
                    {
                        comm.Parameters.Add(new MySqlParameter(kvp.Key, kvp.Value));
                    }

                    conn.Open();
                    using (var reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var element = new T();

                            foreach (var f in properties)
                            {
                                if (!f.PropertyType.IsArray)
                                {
                                    try
                                    {
                                        if (ColumnExists(reader, f.Name))
                                        {
                                            var o = reader[f.Name];
                                            if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        ExceptionLogging.LogError(ex, sql + " " + typeof(T).Name);
                                    }
                                }
                            }

                            conn.Close();
                            return element;
                        }
                    }

                }
            }
            return new T();
        }

        public IEnumerable<T> GetSqlData<T>(string sql, Dictionary<string, object> parmlist) where T : new()
        {
            var properties = typeof(T).GetProperties();
            bool isVirtual;

            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, object> kvp in parmlist)
                    {
                        comm.Parameters.Add(new MySqlParameter(kvp.Key, kvp.Value));
                    }

                    conn.Open();
                    List<T> elements = new List<T>();
                    using (var reader = comm.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            var element = new T();

                            foreach (var f in properties)
                            {

                                isVirtual = typeof(T).GetProperty(f.Name).GetGetMethod().IsVirtual;
                                if (!isVirtual)
                                {
                                    if (!f.PropertyType.IsArray)
                                    {
                                        try
                                        {
                                            if (ColumnExists(reader, f.Name))
                                            {
                                                var o = reader[f.Name];
                                                if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            ExceptionLogging.LogError(ex, sql + " " + typeof(T).Name);
                                        }
                                    }
                                }
                            }

                            elements.Add(element);

                        }
                    }
                    conn.Close();
                    IEnumerable<T> result = elements;
                    return result;
                }
            }
        }

        protected bool ColumnExists(IDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i) == columnName)
                {
                    return true;
                }
            }

            return false;
        }


        public IEnumerable<int> GetSqlData(string sql, Dictionary<string, object> parmlist)
        {
            IList<int> elements = new List<int>();
            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, object> kvp in parmlist)
                    {
                        comm.Parameters.Add(new MySqlParameter(kvp.Key, kvp.Value));
                    }

                    conn.Open();
                    using (var reader = comm.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            elements.Add(reader.GetInt32(0));

                        }
                    }
                    conn.Close();

                    return elements;
                }
            }
        }
        public IEnumerable<T> GetSqlDataNoParms<T>(string sql) where T : new()
        {
            try
            {

                var properties = typeof(T).GetProperties();
                bool isVirtual;

                using (var conn = new MySqlConnection(GetConnectionString))
                {
                    using (var comm = new MySqlCommand(sql, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandTimeout = 1000;
                        conn.Open();
                        List<T> elements = new List<T>();
                        using (var reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var element = new T();

                                foreach (var f in properties)
                                {
                                    isVirtual = typeof(T).GetProperty(f.Name).GetGetMethod().IsVirtual;
                                    if (!isVirtual)
                                    {
                                        if (!f.PropertyType.IsArray)
                                        {
                                            try
                                            {
                                                if (ColumnExists(reader, f.Name))
                                                {
                                                    var o = reader[f.Name];
                                                    if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                ExceptionLogging.LogError(ex, sql + " " + typeof(T).Name);
                                            }
                                        }
                                    }
                                }
                                elements.Add(element);
                            }
                        }
                        conn.Close();
                        IEnumerable<T> result = elements;
                        return result;
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, sql + " " + typeof(T).Name);

                return null;
            }
        }

        public String GetJsonNoParms(string sql)
        {
            try
            {

                using (var conn = new MySqlConnection(GetConnectionString))
                {
                    using (var comm = new MySqlCommand(sql, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandTimeout = 1000;
                        conn.Open();

                        String result = String.Empty;
                        using (var reader = comm.ExecuteReader())
                        {
                            result = DataReaderToJson(reader);
                        }
                        conn.Close();

                        return result;
                    }
                }

            }
            catch (Exception)
            {

                return null;
            }
        }

        public object GetSqlDataSignleValue(string sql, Dictionary<string, object> parmlist, string fieldName)
        {

            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, object> kvp in parmlist)
                    {
                        comm.Parameters.Add(new MySqlParameter(kvp.Key, kvp.Value));
                    }

                    conn.Open();
                    using (var reader = comm.ExecuteReader())
                    {
                        try
                        {
                            while (reader.Read())
                            {
                                return reader[fieldName];
                            }
                            return null;
                        }
                        catch (Exception ex)
                        {
                            ExceptionLogging.LogError(ex, sql);
                            return null;
                        }

                    }
                }

            }



        }

        public int ExecuteStoredProcedureNoParms(string sql)
        {
            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    int rowsAffected = comm.ExecuteNonQuery();
                    conn.Close();
                    return rowsAffected;
                }
            }
        }
        public int ExecuteStoredProcedure(string sql, Dictionary<string, object> parmlist)
        {

            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, object> kvp in parmlist)
                    {
                        comm.Parameters.Add(new MySqlParameter(kvp.Key, kvp.Value));
                    }

                    conn.Open();
                    int rowsAffected = comm.ExecuteNonQuery();
                    conn.Close();
                    return rowsAffected;
                }
            }
        }
        public int ExecuteSql(string sql)
        {

            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    int rowsAffected = comm.ExecuteNonQuery();
                    conn.Close();
                    return rowsAffected;
                }
            }
        }
        private string DataReaderToJson(MySqlDataReader reader)
        {
            StringBuilder sb = new StringBuilder();
            if (reader == null || reader.FieldCount == 0)
            {
                return String.Empty;
            }

            int rowCount = 0;

            sb.Append("[");

            while (reader.Read())
            {
                sb.Append("{");

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    sb.Append(reader.GetName(i) + ":");
                    sb.Append(reader[i] + "");

                    sb.Append(i == reader.FieldCount - 1 ? "" : ",");

                }

                sb.Append("},");

                rowCount++;
            }

            if (rowCount > 0)
            {
                int index = sb.ToString().LastIndexOf(",");
                sb.Remove(index, 1);
            }

            sb.Append("]");
            return sb.ToString();
        }
        public T GetByPrimaryKey<T>(Dictionary<string, object> parmlist) where T : new()
        {
            string sql = typeof(T).Name + "Select";
            var properties = typeof(T).GetProperties();

            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, object> kvp in parmlist)
                    {
                        comm.Parameters.Add(new MySqlParameter(kvp.Key, kvp.Value));
                    }

                    conn.Open();
                    using (var reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var element = new T();

                            foreach (var f in properties)
                            {
                                if (!f.PropertyType.IsArray)
                                {
                                    try
                                    {
                                        if (ColumnExists(reader, f.Name))
                                        {
                                            var o = reader[f.Name];
                                            try
                                            {
                                                if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
                                            }
                                            catch (Exception ex)
                                            {
                                                ExceptionLogging.LogError(ex, sql + " " + typeof(T).Name);
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            conn.Close();
                            return element;
                        }
                    }

                }
            }
            return new T();
        }


        public int AddList(Object o)
        {
            return SaveList(o, "Add");
        }

        public int UpdateList(Object o)
        {
            return SaveList(o, "Update");
        }

        public int AddUpdateList(Object o)
        {
            return SaveList(o, "AddUpdate");
        }
        public int SaveList(Object objects, string actionType)
        {
            string sql = objects.GetType().GetGenericArguments()[0].Name + actionType;
            int rowsAffected = 0;
            IList list = (IList)objects;
            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    foreach (var o in list)
                    {
                        comm.Parameters.Clear();
                        foreach (PropertyInfo pi in o.GetType().GetProperties())
                        {
                            string name = "parm" + pi.Name;
                            comm.Parameters.Add(new MySqlParameter(name, pi.GetValue(o, null)));
                        }
                        if (actionType == "Add" || actionType == "AddUpdate")
                        {
                            comm.Parameters.Add(new MySqlParameter(ParmLastId, MySqlDbType.Int32));
                            comm.Parameters[ParmLastId].Direction = ParameterDirection.Output;
                        }
                        rowsAffected += comm.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }
            return rowsAffected;
        }

        public int Add(Object o)
        {
            string sql = o.GetType().Name + "Add";
            return Save(o, sql, 1);
        }

        public int Update(Object o)
        {
            string sql = o.GetType().Name + "Update";
            return Save(o, sql, 2);
        }

        public int AddUpdate(Object o)
        {
            string sql = o.GetType().Name + "AddUpdate";
            return Save(o, sql, 3);
        }
        public int Remove(Object o)
        {
            string sql = o.GetType().Name + "AddUpdate";
            return Save(o, sql, 3);
        }
        private int Save(Object o, string sql, int actionType)
        {
            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;


                    foreach (PropertyInfo pi in o.GetType().GetProperties())
                    {
                        string name = "parm" + pi.Name;
                        comm.Parameters.Add(new MySqlParameter(name, pi.GetValue(o, null)));
                    }
                    if (actionType == 1 || actionType == 3)
                    {
                        comm.Parameters.Add(new MySqlParameter(ParmLastId, MySqlDbType.Int32));
                        comm.Parameters[ParmLastId].Direction = ParameterDirection.Output;
                    }

                    conn.Open();
                    int rowsAffected = comm.ExecuteNonQuery();
                    conn.Close();
                    if (actionType == 1 || actionType == 3)
                    {
                        int lastInsertID = Convert.ToInt32(comm.Parameters[ParmLastId].Value);
                        return lastInsertID;
                    }
                    else
                    {
                        return rowsAffected;
                    }
                }
            }


        }
        public int ExecuteSPWithOutput(string sql, Dictionary<string, object> parmlist, string outputParm)
        {
            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    foreach (KeyValuePair<string, object> kvp in parmlist)
                    {
                        comm.Parameters.Add(new MySqlParameter(kvp.Key, kvp.Value));
                    }
                    comm.Parameters.Add(new MySqlParameter(outputParm, MySqlDbType.Int32));
                    comm.Parameters[outputParm].Direction = ParameterDirection.Output;
                    conn.Open();
                    int rowsAffected = comm.ExecuteNonQuery();
                    conn.Close();

                    int result = Convert.ToInt32(comm.Parameters[outputParm].Value);
                    return result;

                }
            }


        }

    }
}
