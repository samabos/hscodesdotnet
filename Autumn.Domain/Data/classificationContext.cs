using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Autumn.Domain.Data
{
    public partial class classificationContext : DbContext
    {
        public classificationContext()
        {
        }

        public classificationContext(DbContextOptions<classificationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<Hscode> Hscode { get; set; }
        public virtual DbSet<HscodeToDocument> HscodeToDocument { get; set; }
        public virtual DbSet<Keyworddata> Keyworddata { get; set; }
        public virtual DbSet<Products> Products { get; set; }

        // Unable to generate entity type for table 'dbo.CleanedDescriptions'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.CleanedDescriptionslong'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SAMABOS;Database=classification;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.ApplicationForm).HasMaxLength(255);

                entity.Property(e => e.Code).HasMaxLength(255);

                entity.Property(e => e.Country).HasMaxLength(255);

                entity.Property(e => e.DurationForIssue).HasMaxLength(255);

                entity.Property(e => e.InspectionFee).HasMaxLength(255);

                entity.Property(e => e.Issuer).HasMaxLength(255);

                entity.Property(e => e.LateRenewal).HasMaxLength(255);

                entity.Property(e => e.Parent).HasMaxLength(255);

                entity.Property(e => e.PermitNew).HasMaxLength(255);

                entity.Property(e => e.PermitRenewal).HasMaxLength(255);

                entity.Property(e => e.PnsupportingDocument)
                    .HasColumnName("PNSupportingDocument")
                    .HasMaxLength(255);

                entity.Property(e => e.PrsupportingDocument)
                    .HasColumnName("PRSupportingDocument")
                    .HasMaxLength(255);

                entity.Property(e => e.Remark).HasMaxLength(255);

                entity.Property(e => e.Validity).HasMaxLength(255);
            });

            modelBuilder.Entity<Hscode>(entity =>
            {
                entity.HasKey(e => e.Pid)
                    .HasName("PK__HSCode__C577554061E06720");

                entity.ToTable("HSCode");

                entity.HasIndex(e => e.Order)
                    .HasName("NonClusteredIndex-20191108-152546")
                    .IsUnique();

                entity.Property(e => e.Pid).HasColumnName("PId");

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Parent)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SelfExplanatoryEnglish).IsUnicode(false);

                entity.Property(e => e.SelfExplanatoryFrench).IsUnicode(false);

                entity.Property(e => e.SelfExplanatoryGerman).IsUnicode(false);
            });

            modelBuilder.Entity<HscodeToDocument>(entity =>
            {
                entity.ToTable("HSCodeToDocument");

                entity.Property(e => e.Agency)
                    .HasColumnName("AGENCY")
                    .HasMaxLength(255);

                entity.Property(e => e.Country)
                    .HasColumnName("COUNTRY")
                    .HasMaxLength(255);

                entity.Property(e => e.Description).HasColumnName("DESCRIPTION");

                entity.Property(e => e.ExpGeneral)
                    .HasColumnName("EXP_GENERAL ")
                    .HasMaxLength(255);

                entity.Property(e => e.Hscode)
                    .HasColumnName("HSCODE")
                    .HasMaxLength(255);

                entity.Property(e => e.HscodeLocal)
                    .HasColumnName("HSCODE_LOCAL")
                    .HasMaxLength(255);

                entity.Property(e => e.ImpBulkConsignments)
                    .HasColumnName("IMP_BULK_CONSIGNMENTS")
                    .HasMaxLength(255);

                entity.Property(e => e.ImpChemicalsOrRawMaterials)
                    .HasColumnName("IMP_CHEMICALS_OR_RAW_MATERIALS")
                    .HasMaxLength(255);

                entity.Property(e => e.ImpFinishedProductsInRetailPack)
                    .HasColumnName("IMP_FINISHED_PRODUCTS_IN_RETAIL_PACK")
                    .HasMaxLength(255);

                entity.Property(e => e.ImpGeneral)
                    .HasColumnName("IMP_ GENERAL")
                    .HasMaxLength(255);

                entity.Property(e => e.ImpSupermktOrRestaurant)
                    .HasColumnName("IMP_SUPERMKT_OR_RESTAURANT")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Keyworddata>(entity =>
            {
                entity.ToTable("keyworddata");
            });
            modelBuilder.Entity<Products>(entity =>
            {
                entity.ToTable("Products");
            });
        }
    }
}
