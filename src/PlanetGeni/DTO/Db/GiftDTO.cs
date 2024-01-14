using System;
using System.Collections.Generic;

namespace DTO.Db
{
    public class GiftDTO : UserBankAccountDTO
    {
        public List<int> ToId { get; set; }
        public List<int> NationId { get; set; }
        public List<short> MerchandiseTypeId { get; set; }
    }
}
