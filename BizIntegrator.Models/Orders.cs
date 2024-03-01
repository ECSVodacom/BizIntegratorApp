using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizIntegrator.Models
{
    public class Orders
    {
        public System.Guid Id { get; set; }
        public string OrdNo { get; set; }
        public string OrdDate { get; set; }
        public string OrdDesc { get; set; }
        public string OrdType { get; set; }
        public string OrdTerm { get; set; }
        public string OrdTermDesc { get; set; }
        public string OrdStat { get; set; }
        public string OrderStatus { get; set; }
        public string Origin { get; set; }
        public string PromDate { get; set; }
        public string CompName { get; set; }
        public string BranchNo { get; set; }
        public string BranchName { get; set; }
        public string BranchAddr1 { get; set; }
        public string BranchAddr2 { get; set; }
        public string BranchTel { get; set; }
        public string BranchFax { get; set; }
        public string BranchEmail { get; set; }
        public string BranchVat { get; set; }
        public string VendorRef { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string VendorAddr1 { get; set; }
        public string VendorAddr2 { get; set; }
        public string VendorSuburb { get; set; }
        public string VendorCity { get; set; }
        public string VendorContact { get; set; }
        public string TotLines { get; set; }
        public string TotQty { get; set; }
        public string TotExcl { get; set; }
        public string TotTax { get; set; }
        public string TotVal { get; set; }
        public string DelivAddr1 { get; set; }
        public string DelivAddr2 { get; set; }
        public string DelivSuburb { get; set; }
        public string DelivCity { get; set; }
        public string BuyerNote { get; set; }
        public Nullable<bool> ConfirmInd { get; set; }
        public string CompID { get; set; }
        public Nullable<bool> ResendOrder { get; set; }
        public Nullable<bool> Processed { get; set; }

        public string API_Id { get; set; }
    }
}
