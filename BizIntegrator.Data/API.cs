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
    
    public partial class API
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public API()
        {
            this.Suppliers = new HashSet<Supplier>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string AccountKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string EndPointBase { get; set; }
        public string AuthenticationType { get; set; }
        public string UseAPIKey { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
