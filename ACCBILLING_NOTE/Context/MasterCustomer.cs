//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ACCBILLING_NOTE.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class MasterCustomer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MasterCustomer()
        {
            this.InvoiceHeaders = new HashSet<InvoiceHeader>();
        }
    
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public int MasterInvoiceId { get; set; }
        public string PersonInCharge { get; set; }
        public string TelInside { get; set; }
        public string TelOutside { get; set; }
        public int EnumReturnTypeId { get; set; }
        public Nullable<int> MasterIncotermId { get; set; }
        public bool IsActive { get; set; }
        public int MasterBusinessPartnerId { get; set; }
        public bool IsOversea { get; set; }
        public string CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedUser { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvoiceHeader> InvoiceHeaders { get; set; }
        public virtual MasterInvoice MasterInvoice { get; set; }
    }
}
