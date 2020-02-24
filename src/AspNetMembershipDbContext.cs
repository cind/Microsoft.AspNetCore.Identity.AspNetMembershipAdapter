using Microsoft.EntityFrameworkCore;

namespace Microsoft.AspNetCore.Identity.AspNetMembershipAdapter
{
    public class AspNetMembershipDbContext : DbContext
    {
        public AspNetMembershipDbContext()
            : base()
        {

        }

        public AspNetMembershipDbContext(DbContextOptions<AspNetMembershipDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetUser>()
                .ToTable("aspnet_Users", "dbo");
            modelBuilder.Entity<AspNetUser>()
                .HasKey(u => u.UserId);
            modelBuilder.Entity<AspNetUser>()
                .HasOne(u => u.AspNetMembership).WithOne(m => m.AspNetUser).HasForeignKey<AspNetMembership>(m=>m.UserId).IsRequired();

            modelBuilder.Entity<AspNetMembership>()
                .ToTable("aspnet_Membership", "dbo");
            modelBuilder.Entity<AspNetMembership>()
                .HasKey(m => m.UserId);

            modelBuilder.Entity<AspNetApplication>()
                .ToTable("aspnet_Applications", "dbo");
            modelBuilder.Entity<AspNetApplication>()
                .HasKey(a => a.ApplicationId);
            modelBuilder.Entity<AspNetApplication>()
                .HasMany(a => a.AspNetUsers)
                .WithOne(u => u.AspNetApplication)
                .IsRequired()
                .HasForeignKey(u=>u.ApplicationId);
            modelBuilder.Entity<AspNetApplication>()
                .HasMany(a => a.AspNetMemberships)
                .WithOne(m => m.AspNetApplication)
                .IsRequired()
                .HasForeignKey(m=>m.ApplicationId);
            modelBuilder.Entity<AspNetApplication>()
                .HasMany(a => a.AspNetRoles)
                .WithOne(m => m.AspNetApplication)
                .IsRequired()
                .HasForeignKey(m=>m.ApplicationId);

            modelBuilder.Entity<AspNetRole>()
                .ToTable("aspnet_Roles", "dbo");
            modelBuilder.Entity<AspNetRole>()
                .HasKey(u => u.RoleId);

            modelBuilder.Entity<AspNetUsersInRoles>()
                .ToTable("aspnet_UsersInRoles", "dbo");
            modelBuilder.Entity<AspNetUsersInRoles>()
                .HasKey(ur => new {ur.UserID, ur.RoleID});
            modelBuilder.Entity<AspNetUsersInRoles>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.AspNetUsersInRoles)
                .HasForeignKey(ur => ur.UserID)
                .IsRequired();
            modelBuilder.Entity<AspNetUsersInRoles>()
                .HasOne(ur => ur.Role)
                .WithMany(u => u.AspNetUsersInRoles)
                .HasForeignKey(ur => ur.RoleID)
                .IsRequired();
        }

        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<AspNetMembership> AspNetMemberships { get; set; }
        public DbSet<AspNetApplication> AspNetApplications { get; set; }
    }
}
