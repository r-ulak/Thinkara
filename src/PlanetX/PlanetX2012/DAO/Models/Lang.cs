using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Lang
    {
        public sbyte LanguageId { get; set; }
        public string Lang1 { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}