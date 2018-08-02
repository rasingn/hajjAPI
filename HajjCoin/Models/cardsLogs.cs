namespace HajjCoin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cardsLogs
    {
        public Guid CardID { get; set; }

        public int NoOfCoins { get; set; }

        public DateTime? ModificationDate { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LogID { get; set; }

        public virtual configCards configCards { get; set; }
    }
}
