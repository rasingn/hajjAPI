namespace HajjCoin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class configCards
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public configCards()
        {
            cardsLogs = new HashSet<cardsLogs>();
            Coins = new HashSet<Coins>();
        }

        [Key]
        public Guid CardID { get; set; }

        public int? HolderID { get; set; }

        public int? NoOfCoins { get; set; }

        public DateTime? ActionDate { get; set; }

        public int? BranchID { get; set; }

        public virtual Branches Branches { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cardsLogs> cardsLogs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Coins> Coins { get; set; }

        public virtual HoldersDetails HoldersDetails { get; set; }
    }
}
