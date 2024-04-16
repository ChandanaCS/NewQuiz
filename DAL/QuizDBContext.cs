using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class QuizDBContext : DbContext
    {
        public QuizDBContext(DbContextOptions<QuizDBContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Organisation> Organisations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired();

                entity.Property(e => e.EmailId)
                      .IsRequired();

                entity.Property(e => e.Password)
                      .IsRequired()
                      .HasMaxLength(15);

                entity.Property(e => e.OrganisationId)
                      .IsRequired();

                entity.Property(e => e.PhoneNumber)
                      .IsRequired()
                      .HasMaxLength(10);

                entity.Property(e => e.IsApproved)
                      .HasDefaultValue(false);

                entity.Property(e => e.ModifiedDate)
                      .IsRequired(false);

                entity.Property(e => e.ModifiedBy)
                      .IsRequired(false);

                entity.Property(e => e.RoleId)
                      .HasDefaultValue(3);

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true);
            });
        }
    }
}
