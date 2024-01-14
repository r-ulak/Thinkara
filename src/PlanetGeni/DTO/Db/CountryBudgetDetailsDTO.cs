using System;
namespace DTO.Db
{
    public class CountryBudgetDetailsDTO
    {
        public Guid TaskId{ get; set; }
        public string CountryId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountLeft { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Status { get; set; }
        public bool AllowEdit { get; set; }
        public CountryBudgetTypeDTO[] BudgetType { get; set; }
    }


}
