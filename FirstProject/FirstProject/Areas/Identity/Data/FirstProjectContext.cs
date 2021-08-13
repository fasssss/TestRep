﻿using System;
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
        public DbSet<PollsHistoryModel> PollsHistory { get; set; }
        public DbSet<VotesHistoryModel> VotesInPollsHistory { get; set; }
        public DbSet<VotesTypesModel> VotesTypes { get; set; }
        public DbSet<FileInDbModel> FilesInDb { get; set; }
        public DbSet<StatusTypesModel> StatusTypes { get; set; }
        public DbSet<ChatHistoryModel> ChatHistory { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AuthorityDependenciesModel>().HasKey(c => new { c.AuthrityId, c.RepresentativeAuthrityId });
            builder.Entity<VoteModel>().HasKey(c => new { c.QuestionId, c.UserId });

            builder.Entity<PolleModel>().Property(c => c.Description).IsRequired();
            builder.Entity<PolleModel>().Property(c => c.StatusId).IsRequired();
            builder.Entity<QuestionModel>().Property(c => c.PolleId).IsRequired();
            builder.Entity<QuestionModel>().Property(c => c.Question).IsRequired();
            builder.Entity<VoteModel>().Property(c => c.UserId).IsRequired();
            builder.Entity<VoteModel>().Property(c => c.QuestionId).IsRequired();
            builder.Entity<VoteModel>().Property(c => c.VoteTypeId).IsRequired();
            builder.Entity<VotesTypesModel>().Property(c => c.VoteName).IsRequired();
            builder.Entity<StatusTypesModel>().Property(c => c.StatusName).IsRequired();
            builder.Entity<FileInDbModel>().Property(c => c.ContentType).IsRequired();
            builder.Entity<FileInDbModel>().Property(c => c.File).IsRequired();
            builder.Entity<FileInDbModel>().Property(c => c.FileName).IsRequired();
            builder.Entity<FileInDbModel>().Property(c => c.QuestionId).IsRequired();
            builder.Entity<FileInDbModel>().HasOne(c => c.Question).WithOne(c => c.File).OnDelete(DeleteBehavior.Cascade);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
