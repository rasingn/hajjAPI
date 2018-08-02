namespace HajjCoin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transaction")]
    public partial class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }

        public int? SupplyerID { get; set; }

        public DateTime? TransactionDate { get; set; }

        public int? TotalCoinsUsed { get; set; }

        public virtual Suppliers Suppliers { get; set; }
    }
}
