using System;

namespace BizIntegrator.InvoiceManager.Models
{
    public class Invoice
    {
        public string InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public int DocumentState { get; set; }
        public string OrderNumber { get; set; }
        public string ExternalOrderNumber { get; set; }
        public string CustomerCode { get; set; }
        public double GrossTotalInVat { get; set; }
        public double GrossTotalExVat { get; set; }
        public double GrossTaxTotal { get; set; }
        public double DiscountAmountInVat { get; set; }
        public double DiscountAmountExVat { get; set; }
        public double NetTotalExVat { get; set; }
        public double NetTaxTotal { get; set; }
        public int TotalInvoiceRounding { get; set; }
        public double NetTotalInVat { get; set; }
       
    }
}
