namespace HajjCoin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Suppliers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Suppliers()
        {
            Transaction = new HashSet<Transaction>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SupplierID { get; set; }

        [StringLength(50)]
        public string SupplierName { get; set; }

        public long? BankCard { get; set; }

        public int? ServiceTypeID { get; set; }

        public DateTime? SpplierRegestrationDate { get; set; }

        [StringLength(250)]
        public string SpplierEmail { get; set; }

        public string SupplierPwd { get; set; }

        public int? CountryID { get; set; }

        public virtual Countries Countries { get; set; }

        public virtual ServiceType ServiceType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
