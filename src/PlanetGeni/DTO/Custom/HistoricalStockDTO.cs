using System;
namespace DTO.Custom
{
    public class HistoricalStockDTO
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public double Adj_Close { get; set; }
    }
}
