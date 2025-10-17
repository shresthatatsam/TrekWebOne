using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserRoles.configuration;
using UserRoles.Models;
using UserRoles.Models.Blog;
using UserRoles.Models.Contact;
using UserRoles.Models.Trek;

namespace UserRoles.Data
{
    public class AppDbContext : IdentityDbContext<Users>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<CarousalImage> CarousalImages { get; set; }
        public DbSet<Deals> Deals { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<PageContent> PageContents { get; set; }
        public DbSet<NabBarContent> NavBarContents { get; set; }
        public DbSet<NavItem> NavItems { get; set; }

        public DbSet<TrekPackage> TrekPackages { get; set; }
        public DbSet<TrekPackageImage> TrekPackageImages { get; set; }
        public DbSet<TrekPackageCostInfo> TrekPackageCostInfos { get; set; }
        public DbSet<TrekPackageGroupPricing> TrekPackageGroupPricings { get; set; }
        public DbSet<TrekFAQ> TrekFAQs { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<TrekItineraryDay> TrekItineraryDays { get; set; }
        public DbSet<TrekPackageFixedPricing> TrekPackageFixedPricings { get; set; }


        //public DbSet<TrekPackageMapImage> TrekPackageMapImages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
            //modelBuilder.Ignore<IdentityUser>();
            //modelBuilder.Ignore<IdentityRole>();
            //modelBuilder.Ignore<IdentityUserRole<string>>();
            //modelBuilder.Ignore<IdentityUserClaim<string>>();
            //modelBuilder.Ignore<IdentityUserLogin<string>>();
            //modelBuilder.Ignore<IdentityRoleClaim<string>>();
            //modelBuilder.Ignore<IdentityUserToken<string>>();

            //modelBuilder.Ignore<CarousalImage>();
            //modelBuilder.Ignore<Deals>();
            ////modelBuilder.Ignore<AboutUs>();
            //modelBuilder.Ignore<TeamMember>();
            //modelBuilder.Ignore<PageContent>();
            //modelBuilder.Ignore<NabBarContent>();
            //modelBuilder.Ignore<NavItem>();

            //modelBuilder.Ignore<TrekPackage>();
            //modelBuilder.Ignore<TrekPackageImage>();
            //modelBuilder.Ignore<TrekPackageCostInfo>();
            //modelBuilder.Ignore<TrekPackageGroupPricing>();
            //modelBuilder.Ignore<TrekFAQ>();
            //modelBuilder.Ignore<TrekItineraryDay>();
            //modelBuilder.Ignore<TrekPackageFixedPricing>();
            modelBuilder.ApplyConfiguration(new CarousalImageConfiguration());
        }

    }
}
