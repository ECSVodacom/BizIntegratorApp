namespace BizIntegrator.Models
{
    public class StockBarcode
    {
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string BottleBarcode { get; set; }
        public string CaseBarcode { get; set; }
        public string BottleUom { get; set; }
        public string CaseUom { get; set; }
        public int UnitsPerCase { get; set; }
    }


}
