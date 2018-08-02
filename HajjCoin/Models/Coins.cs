namespace HajjCoin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Coins
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Coins()
        {
            TransactionsCoinsSupplier = new HashSet<TransactionsCoinsSupplier>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CoinID { get; set; }

        public Guid CardID { get; set; }

        public int? Coin { get; set; }

        public virtual configCards configCards { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransactionsCoinsSupplier> TransactionsCoinsSupplier { get; set; }
    }
}
