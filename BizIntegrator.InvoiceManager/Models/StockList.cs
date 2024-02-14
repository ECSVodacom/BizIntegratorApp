namespace BizIntegrator.InvoiceManager.Models
{
    public class StockList
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string AlternateName { get; set; }
        public double PriceInVat { get; set; }
        public double PriceExVat { get; set; }
        public string BottleBarcode { get; set; }
        public string CaseBarcode { get; set; }
    }


}
