using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Category
{
    public static class UnitUtility
    {
        public static int ElmahErrorCount(StoredProcedureExtender spContext)
        {
            int count =
               Convert.ToInt32(spContext.GetSqlDataSignleValue("ElmahErrorCount", new Dictionary<string, object>(), "cnt"));
            return count;

        }

        public static string LoginPassword(int userId)
        {
            StringBuilder pwd = new StringBuilder("Kathmandu");
            pwd.Append(DateTime.UtcNow.Day + DateTime.UtcNow.Month);
            return pwd.ToString();
        }
    }
}
