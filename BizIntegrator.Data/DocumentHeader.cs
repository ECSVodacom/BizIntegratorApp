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
    
    public partial class DocumentHeader
    {
        public System.Guid Id { get; set; }
        public System.Guid Sender_Id { get; set; }
        public System.Guid Receiver_Id { get; set; }
        public Nullable<System.Guid> DocumentIdentification_Id { get; set; }
        public Nullable<System.Guid> Manifest_Id { get; set; }
        public Nullable<System.Guid> Order_Id { get; set; }
        public Nullable<System.Guid> Supplier_Id { get; set; }
    
        public virtual DocumentIdentification DocumentIdentification { get; set; }
        public virtual Order Order { get; set; }
        public virtual Receiver Receiver { get; set; }
        public virtual Sender Sender { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
