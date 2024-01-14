using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DAO.DAO
{
    public class EnumClass
    {
        public enum UserStatus
        {
            Offline = 0,
            Online = 1,
            Away = 2,
            Busy = 3,
            AppearOffline = 4

        }
    }
}