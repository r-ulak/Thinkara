using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PlanetX2012.Models.DAO;
using DAO.Models;

namespace PlanetX2012.Models.ContentModel
{
    public class BusinessContent
    {
        
        public Business Business { get; set; }        
        public bool CreateModal { get; set; }
        public BusinessContent()
        {

        }

    }
}