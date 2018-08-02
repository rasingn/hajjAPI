namespace HajjCoin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransactionsCoinsSupplier")]
    public partial class TransactionsCoinsSupplier
    {
        public TransactionsCoinsSupplier()
        {

        }

        public TransactionsCoinsSupplier(int TransactionID)
        {
            this.TransactionID = TransactionID;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TransactionID { get; set; }

        public int CoinsID { get; set; }

        public virtual Coins Coins { get; set; }
    }
}
