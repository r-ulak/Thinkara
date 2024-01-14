using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class StockForecastDTO : Stock
    {
        public decimal ForecastValue { get; set; }
        public string Ratings { get; set; }
    }
}
