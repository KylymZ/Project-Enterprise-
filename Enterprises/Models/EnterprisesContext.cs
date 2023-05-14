using System;

using Enterprises;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Enterprises
{
    public partial class EnterprisesContext : DbContext
    {
		
		public EnterprisesContext()
		{
			
		}

		public EnterprisesContext(DbContextOptions<EnterprisesContext> options)
		: base(options)
		{
		}
    
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
					.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
					.AddJsonFile("appsettings.json")
					.Build();

			string connectionString = configuration.GetConnectionString("DefaultConnection");
			optionsBuilder.UseSqlServer(connectionString);
		}

		public virtual DbSet<Budgets> Budgets { get; set; }
        public virtual DbSet<Months> Months { get; set; }
        public virtual DbSet<Years> Years { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<Finproducts> Finproducts { get; set; }
        public virtual DbSet<Ingredients> Ingredients { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }
        public virtual DbSet<Production> Production { get; set; }
        public virtual DbSet<PurchaseOfrawmaterials> PurchaseOfrawmaterials { get; set; }
        public virtual DbSet<Rawmaterials> Rawmaterials { get; set; }
        public virtual DbSet<Saleofproducts> Saleofproducts { get; set; }
        public virtual DbSet<SalaryEmp> Salary { get; set; }
        public virtual DbSet<Units> Units { get; set; }



        //public virtual DbSet<ViewEmployees> ViewEmployees { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Budgets>();

        //    modelBuilder.Entity<Months>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.Property(e => e.MonthName).HasMaxLength(50);

        //    });

        //    modelBuilder.Entity<Years>(entity =>
        //    {

        //        entity.Property(e => e.YearName).HasColumnType("int");

        //    });

        //    modelBuilder.Entity<Employees>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.Property(e => e.Address).HasMaxLength(50);

        //        entity.Property(e => e.Fullname).HasMaxLength(50);

        //        entity.Property(e => e.Salary).HasColumnType("money");

        //        entity.Property(e => e.Telephone).HasMaxLength(50);

        //        entity.HasOne(d => d.PositionNavigation)
        //            .WithMany(p => p.Employees)
        //            .HasForeignKey(d => d.Position)
        //            .HasConstraintName("FK_Employees_Positions");
        //    });

        //    modelBuilder.Entity<Finproducts>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.Property(e => e.Name).HasMaxLength(50);

        //        entity.Property(e => e.Sum).HasColumnType("money");

        //        entity.HasOne(d => d.UnitNavigation)
        //            .WithMany(p => p.Finproducts)
        //            .HasForeignKey(d => d.Unit)
        //            .HasConstraintName("FK_Finproducts_Units");
        //    });

        //    modelBuilder.Entity<Ingredients>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.HasOne(d => d.ProductNavigation)
        //            .WithMany(p => p.Ingredients)
        //            .HasForeignKey(d => d.Product)
        //            .HasConstraintName("FK_Ingredients_Finproducts");

        //        entity.HasOne(d => d.RawMaterialsNavigation)
        //            .WithMany(p => p.Ingredients)
        //            .HasForeignKey(d => d.RawMaterials)
        //            .HasConstraintName("FK_Ingredients_Rawmaterials");
        //    });

        //    modelBuilder.Entity<Positions>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.Property(e => e.Position).HasMaxLength(50);
        //    });

        //    modelBuilder.Entity<Production>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.Property(e => e.Date).HasColumnType("smalldatetime");

        //        entity.HasOne(d => d.EmployeeNavigation)
        //            .WithMany(p => p.Production)
        //            .HasForeignKey(d => d.Employee)
        //            .HasConstraintName("FK_Production_Employees");

        //        entity.HasOne(d => d.ProductNavigation)
        //            .WithMany(p => p.Production)
        //            .HasForeignKey(d => d.Product)
        //            .HasConstraintName("FK_Production_Finproducts");
        //    });

        //    modelBuilder.Entity<PurchaseOfrawmaterials>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.Property(e => e.Date).HasColumnType("smalldatetime");

        //        entity.Property(e => e.Sum).HasColumnType("money");

        //        entity.HasOne(d => d.EmployeeNavigation)
        //            .WithMany(p => p.PurchaseOfrawmaterials)
        //            .HasForeignKey(d => d.Employee)
        //            .HasConstraintName("FK_PurchaseOfrawmaterials_Employees");

        //        entity.HasOne(d => d.RawMaterialsNavigation)
        //            .WithMany(p => p.PurchaseOfrawmaterials)
        //            .HasForeignKey(d => d.RawMaterials)
        //            .HasConstraintName("FK_PurchaseOfrawmaterials_Rawmaterials");
        //    });


        //    modelBuilder.Entity<SalaryEmp>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.HasOne(d => d.YearNavigation)
        //           .WithMany(p => p.SalaryEmp)
        //           .HasForeignKey(d => d.Year)
        //           .HasConstraintName("FK_Salary_Years");

        //        entity.HasOne(d => d.MonthNavigation)
        //           .WithMany(p => p.SalaryEmp)
        //           .HasForeignKey(d => d.Month)
        //           .HasConstraintName("FK_Salary_Months");


        //        entity.HasOne(d => d.EmployeeNavigation)
        //            .WithMany(p => p.Salaries)
        //            .HasForeignKey(d => d.Employee)
        //            .HasConstraintName("FK_Salary_Employees");


        //        entity.Property(e => e.ParticipationPurchase).HasColumnType("tinyint");

        //        entity.Property(e => e.ParticipationSale).HasColumnType("tinyint");

        //        entity.Property(e => e.ParticipationProduction).HasColumnType("tinyint");

        //        entity.Property(e => e.CountParticipation).HasComputedColumnSql("(([ParticipationPurchase]+[ParticipationSale])+[ParticipationProduction])");

        //        entity.Property(e => e.SalaryEmployee).HasColumnType("money");

        //        entity.Property(e => e.TotalAmount).HasComputedColumnSql("([SalaryEmployee]+[Bonus])");

        //        entity.Property(e => e.Issued).HasColumnType("bit");

        //        entity.Property(e => e.Bonus).HasColumnType("real");
        //    });

        //    modelBuilder.Entity<Saleofproducts>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.Property(e => e.Date).HasColumnType("smalldatetime");

        //        entity.Property(e => e.Sum).HasColumnType("money");

        //        entity.HasOne(d => d.EmployeeNavigation)
        //            .WithMany(p => p.Saleofproducts)
        //            .HasForeignKey(d => d.Employee)
        //            .HasConstraintName("FK_Saleofproducts_Employees");

        //        entity.HasOne(d => d.ProductNavigation)
        //            .WithMany(p => p.Saleofproducts)
        //            .HasForeignKey(d => d.Product)
        //            .HasConstraintName("FK_Saleofproducts_Finproducts");
        //    });


        //    modelBuilder.Entity<Rawmaterials>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.Property(e => e.Name).HasMaxLength(50);

        //        entity.Property(e => e.Sum).HasColumnType("money");

        //        entity.HasOne(d => d.UnitNavigation)
        //            .WithMany(p => p.Rawmaterials)
        //            .HasForeignKey(d => d.Unit)
        //            .HasConstraintName("FK_Rawmaterials_Units");
        //    });

        //    modelBuilder.Entity<Saleofproducts>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.Property(e => e.Date).HasColumnType("smalldatetime");

        //        entity.Property(e => e.Sum).HasColumnType("money");

        //        entity.HasOne(d => d.EmployeeNavigation)
        //            .WithMany(p => p.Saleofproducts)
        //            .HasForeignKey(d => d.Employee)
        //            .HasConstraintName("FK_Saleofproducts_Employees");

        //        entity.HasOne(d => d.ProductNavigation)
        //            .WithMany(p => p.Saleofproducts)
        //            .HasForeignKey(d => d.Product)
        //            .HasConstraintName("FK_Saleofproducts_Finproducts");
        //    });

        //    modelBuilder.Entity<Units>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.Property(e => e.Name).HasMaxLength(50);
        //    });

        //    modelBuilder.Entity<ViewEmployees>(entity =>
        //    {
        //        entity.HasNoKey();

        //        entity.ToView("View_Employees");

        //        entity.Property(e => e.Address).HasMaxLength(50);

        //        entity.Property(e => e.Fullname).HasMaxLength(50);

        //        entity.Property(e => e.Id).HasColumnName("ID");

        //        entity.Property(e => e.Position).HasMaxLength(50);

        //        entity.Property(e => e.Salary).HasColumnType("money");

        //        entity.Property(e => e.Telephone).HasMaxLength(50);
        //    });

        //    OnModelCreatingPartial(modelBuilder);
        //}

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
