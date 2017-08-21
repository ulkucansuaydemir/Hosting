namespace kayzer.project.Context
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using kayzer.project.Models;
    using kayzer.project.Migrations;

    public partial class KayzerDbContext : DbContext
    {
        public KayzerDbContext()
            : base("name=KayzerDbContext")
        {
        }
        public virtual DbSet<UserModel> Users { get; set; }
        public virtual DbSet<ProcessorModel> Processors { get; set; }
        public virtual DbSet<InternetAccessModel> InternetAccesses { get; set; }
        public virtual DbSet<RamModel> Rams { get; set; }
        public virtual DbSet<HardDiskModel> HardDisks { get; set; }
        public virtual DbSet<ProductModel> Product { get; set; }
        public virtual DbSet<ExtraProductModel> ExtraProduct { get; set; }
        public virtual DbSet<DefaulProductModel> DefaultProduct { get; set; }
        public virtual DbSet<OrderModel> Orders { get; set; }
        public virtual DbSet<ColocationModel> CoLocations { get; set; }
        public virtual DbSet<OrderColocationModel> OrderColocation { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<ProductModel>().HasMany(t => t.Processor).WithMany(t => t.Product).Map(m =>
            {
                 m.ToTable("ProductProcessorModel");
                 m.MapLeftKey("ProductID");
                 m.MapRightKey("ProcessorID");
            });

            modelBuilder.Entity<ProductModel>().HasMany(t => t.Ram).WithMany(t => t.Product).Map(m =>
            {
                m.ToTable("ProductRamModel");
                m.MapLeftKey("ProductID");
                m.MapRightKey("RamID");
            });

            modelBuilder.Entity<ProductModel>().HasMany(t => t.IntAcces).WithMany(t => t.Product).Map(m =>
            {
                m.ToTable("ProductIntAccModel");
                m.MapLeftKey("ProductID");
                m.MapRightKey("IntAccID");
            });

            modelBuilder.Entity<ProductModel>().HasMany(t => t.HardDisk).WithMany(t => t.Product).Map(m =>
            {
                m.ToTable("ProductHardDiskModel");
                m.MapLeftKey("ProductID");
                m.MapRightKey("HardDiskID");
            });


            Database.SetInitializer<KayzerDbContext>(new MigrateDatabaseToLatestVersion<KayzerDbContext, Configuration>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
