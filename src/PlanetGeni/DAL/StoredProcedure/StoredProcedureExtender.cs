using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    //Only Intented to be used with Unit Test as it uses a plain sql.
    public class StoredProcedureExtender : StoredProcedure
    {
        public StoredProcedureExtender()
            : base()
        { }
        public StoredProcedureExtender(string connectionStringKey)
            : base(connectionStringKey)
        { }
        public IEnumerable<T> GetSqlData<T>(string sql) where T : new()
        {
            var properties = typeof(T).GetProperties();
            bool isVirtual;

            using (var conn = new MySqlConnection(GetConnectionString))
            {
                using (var comm = new MySqlCommand(sql, conn))
                {
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

    }
}
