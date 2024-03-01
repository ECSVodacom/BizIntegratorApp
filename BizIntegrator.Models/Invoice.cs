using System;

namespace BizIntegrator.Models
{
    public class Invoice
    {
        public string InvoiceNumber { get; set; }
        public int InvoiceId { get; set; }
        
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
                public Nullable<bool> Processed { get; set; }

        public string API_Id { get; set; }
       
    }
}
