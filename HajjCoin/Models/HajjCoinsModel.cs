namespace HajjCoin.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HajjCoinsModel : DbContext
    {
        public HajjCoinsModel()
            : base("name=HajjCoinsEntities")
        {
        }

        public virtual DbSet<Branches> Branches { get; set; }
        public virtual DbSet<cardsLogs> cardsLogs { get; set; }
        public virtual DbSet<Coins> Coins { get; set; }
        public virtual DbSet<configCards> configCards { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<HoldersDetails> HoldersDetails { get; set; }
        public virtual DbSet<ServiceType> ServiceType { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionsCoinsSupplier> TransactionsCoinsSupplier { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coins>()
                .HasMany(e => e.TransactionsCoinsSupplier)
                .WithRequired(e => e.Coins)
                .HasForeignKey(e => e.CoinsID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<configCards>()
                .HasMany(e => e.cardsLogs)
                .WithRequired(e => e.configCards)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<configCards>()
                .HasMany(e => e.Coins)
                .WithRequired(e => e.configCards)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HoldersDetails>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Suppliers>()
                .HasMany(e => e.Transaction)
                .WithOptional(e => e.Suppliers)
                .HasForeignKey(e => e.SupplyerID);
        }
    }
}
