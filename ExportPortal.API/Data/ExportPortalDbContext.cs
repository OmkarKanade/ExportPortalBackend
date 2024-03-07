using ExportPortal.API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ExportPortal.API.Data
{
    public class ExportPortalDbContext : IdentityDbContext<UserProfile>
    {
        public ExportPortalDbContext(DbContextOptions<ExportPortalDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<VendorCategory> VendorCategories { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationItem> QuotationItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var adminRoleId = "7f541d69-7524-4077-bcb8-bdbe3fd836e0";
            var vendorRoleId = "1beefd77-dac2-4b30-b285-4407bfd1507f";
            var customerRoleId = "8bb312cb-0bbc-4788-9b55-8520aaa01e35";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id=adminRoleId,
                    ConcurrencyStamp=adminRoleId,
                    Name="Admin",
                    NormalizedName="Admin".ToUpper()
                },
                new IdentityRole
                {
                    Id=vendorRoleId,
                    ConcurrencyStamp=vendorRoleId,
                    Name="Vendor",
                    NormalizedName="Vendor".ToUpper()
                },
                new IdentityRole
                {
                    Id=customerRoleId,
                    ConcurrencyStamp=customerRoleId,
                    Name="Customer",
                    NormalizedName="Customer".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            builder.Entity<UserProfile>().Navigation(e => e.VendorCategory).AutoInclude();
            builder.Entity<Product>().HasOne(u => u.UserProfile1).WithMany().HasForeignKey(u => u.VendorId1);
            builder.Entity<Product>().HasOne(u => u.UserProfile2).WithMany().HasForeignKey(u => u.VendorId2);
            builder.Entity<Product>().HasOne(u => u.UserProfile3).WithMany().HasForeignKey(u => u.VendorId3);
            
            
            builder.Entity<Quotation>().HasMany(q => q.Items).WithOne(p => p.Quotation).HasForeignKey(p => p.QuotationId);
            builder.Entity< QuotationItem >().HasOne(q => q.Quotation).WithMany(p => p.Items).HasForeignKey(q => q.QuotationId);


        }

    }
}
