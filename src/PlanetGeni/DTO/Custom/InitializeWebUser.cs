using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Custom
{
    public class InitializeWebUser
    {
        public WebUser UserInfo { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSeceret { get; set; }
        public string YahooUserGuid { get; set; }
        public string LoginProvider { get; set; }
    }
}
