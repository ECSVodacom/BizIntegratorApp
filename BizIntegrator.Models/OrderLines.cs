using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizIntegrator.Models
{
    public class OrderLines
    {
        public string OrdLn { get; set; }
        public string OrdNo { get; set; }
        public string ItemNo { get; set; }
        public string ItemDesc { get; set; }
        public string MfrItem { get; set; }
        public string QtyConv { get; set; }
        public string OrdQty { get; set; }
        public string PurcUom { get; set; }
        public string PurcUomConv { get; set; }
        public string TaxCde { get; set; }
        public string TaxRate { get; set; }
        public string UnitPrc { get; set; }
        public string LineTotExcl { get; set; }
        public string LineTotTax { get; set; }
        public string LineTotVal { get; set; }
    }
}
