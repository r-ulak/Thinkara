using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.ComponentModel;

namespace PlanetX2012.Models.DAO
{
    public class StoredProcedure
    {
        # region Properties
        private string connectionString { get; set; }
        private bool alreadyLoadedconnectionString { get; set; }
        private string GetConnectionString
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
                connectionString = ConfigurationManager.ConnectionStrings["MySQLConnectionString"].ConnectionString;
            }
            alreadyLoadedconnectionString = true;
        }

        #endregion

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
                                var o = reader[f.Name];
                                if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
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
                                    var o = reader[f.Name];
                                    if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
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
                                    var o = reader[f.Name];
                                    if (o.GetType() != typeof(DBNull)) f.SetValue(element, o, null);
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
                        reader.Read();
                        return reader[fieldName];
                    }
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
    }
}
