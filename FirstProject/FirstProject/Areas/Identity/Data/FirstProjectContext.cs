using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FirstProject.Models;

namespace FirstProject.Data
{
    public class FirstProjectContext : IdentityDbContext<ExtendedUserModel, IdentityRole<System.Guid>, System.Guid, IdentityUserClaim<System.Guid>,
        IdentityUserRole<System.Guid>, IdentityUserLogin<System.Guid>, IdentityRoleClaim<System.Guid>, IdentityUserToken<System.Guid>>
    {
        public FirstProjectContext(DbContextOptions<FirstProjectContext> options)
            : base(options)
        {
        }

        public DbSet<FileModel> Files { get; set; }
        public DbSet<LocaleModel> Locales { get; set; }
        public DbSet<AuthorityDependenciesModel> AuthorityDependencies { get; set; }

        public DbSet<PolleModel> Polles { get; set; }
        public DbSet<QuestionModel> Questions { get; set; }
        public DbSet<VoteModel> Votes { get; set; }
        public DbSet<VotesHistoryModel> History { get; set; }
        public DbSet<VotesTypesModel> VotesTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AuthorityDependenciesModel>().HasKey(c => new { c.AuthrityId, c.RepresentativeAuthrityId });
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
