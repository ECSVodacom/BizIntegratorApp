namespace BizIntegrator.Models
{
    public class InvoiceLine
    {
        public string InvoiceNumber { get; set; }
        public int InvoiceId { get; set; }
        public string WarehouseCode { get; set; }
        public string ItemCode { get; set; }
        public string ModuleCode { get; set; }
        public string LineDescription { get; set; }
        public double UnitPriceExcl { get; set; }
        public double UnitPriceIncl { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public double LineNetTotalOrderedInVat { get; set; }
        public double LineNetTotalOrderedExVat { get; set; }
        public double LineNetTotalProcessedInVat { get; set; }
        public double LineNetTotalProcessedExVat { get; set; }
        public string LineNotes { get; set; }
    }
}
