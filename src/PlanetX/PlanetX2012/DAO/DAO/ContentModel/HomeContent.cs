using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.DAO;
using DAO.Models;
using PlanetX2012.Models.DAO;


namespace PlanetX2012.Models.ContentModel
{
    public class HomeContent
    {
        public FinanceContent finance { get; set; }
        public WebUser useraccount { get; set; }
        public ProfileSummary profileSummary { get; set; }
        public string city { get; set; }
    }
}