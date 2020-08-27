using Churchgoers.Common.Entities;
using Churchgoers.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Churchgoers.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Church> Churches { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<Field> Fields { get; set; }

        public DbSet<Profession> Professions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Field>(fie =>
            {
                fie.HasIndex("Name").IsUnique();
                fie.HasMany(f => f.Districts).WithOne(d => d.Field).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<District>(dis =>
            {
                dis.HasIndex("Name", "FieldId").IsUnique();
                dis.HasOne(d => d.Field).WithMany(f => f.Districts).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Church>(chu =>
            {
                chu.HasIndex("Name", "DistrictId").IsUnique();
                chu.HasOne(c => c.District).WithMany(d => d.Churches).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
