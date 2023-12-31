//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BizIntegrator.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.ConfirmedOrders = new HashSet<ConfirmedOrder>();
            this.ConfirmOrderRequests = new HashSet<ConfirmOrderRequest>();
            this.DocumentHeaders = new HashSet<DocumentHeader>();
            this.Invoices = new HashSet<Invoice>();
            this.UploadedInvoices = new HashSet<UploadedInvoice>();
        }
    
        public System.Guid Id { get; set; }
        public string OrdNo { get; set; }
        public Nullable<System.DateTime> OrdDate { get; set; }
        public string OrdDesc { get; set; }
        public string OrdType { get; set; }
        public string OrdTerm { get; set; }
        public string OrdTermDesc { get; set; }
        public Nullable<int> OrdStat { get; set; }
        public string OrderStatus { get; set; }
        public string Origin { get; set; }
        public Nullable<System.DateTime> PromDate { get; set; }
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
        public Nullable<int> TotLines { get; set; }
        public Nullable<decimal> TotQty { get; set; }
        public Nullable<decimal> TotExcl { get; set; }
        public Nullable<decimal> TotTax { get; set; }
        public Nullable<decimal> TotVal { get; set; }
        public string DelivAddr1 { get; set; }
        public string DelivAddr2 { get; set; }
        public string DelivSuburb { get; set; }
        public string DelivCity { get; set; }
        public string BuyerNote { get; set; }
        public Nullable<bool> ConfirmInd { get; set; }
        public string CompID { get; set; }
        public Nullable<bool> ResendOrder { get; set; }
        public Nullable<bool> Processed { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfirmedOrder> ConfirmedOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfirmOrderRequest> ConfirmOrderRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentHeader> DocumentHeaders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invoice> Invoices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UploadedInvoice> UploadedInvoices { get; set; }
    }
}
