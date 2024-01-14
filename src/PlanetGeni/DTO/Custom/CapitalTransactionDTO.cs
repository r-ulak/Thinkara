using DAO.Models;
using System;
namespace DTO.Db
{
    public class CapitalTransactionDTO : WebUserDTO
    {
        public int SourceId { get; set; }
        public int RecipentId { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public sbyte FundType { get; set; }
        public System.DateTime CreatedAT { get; set; }
    }
}
