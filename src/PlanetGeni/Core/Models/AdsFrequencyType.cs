using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class AdsFrequencyType
    {
        public sbyte AdsFrequencyTypeId { get; set; }
        public string FrequencyName { get; set; }
        public sbyte FrequencyMultiple { get; set; }
    }
}
