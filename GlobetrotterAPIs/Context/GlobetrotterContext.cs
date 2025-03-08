using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using GlobetrotterAPIs.Models;

namespace GlobetrotterAPIs.Context
{
    public partial class GlobetrotterContext : DbContext
    {
        public GlobetrotterContext()
        {
        }

        public GlobetrotterContext(DbContextOptions<GlobetrotterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Challenge> Challenges { get; set; } = null!;
        public virtual DbSet<Destination> Destinations { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=globetrotter.database.windows.net,1433;Database=Globetrotter;User Id=GlobetrotterUser;password=MyDbPassword1;Trusted_Connection=False;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Challenge>(entity =>
            {
                entity.HasKey(e => e.InviteCode)
                    .HasName("PK__challeng__C90895D5913A0F82");

                entity.ToTable("challenges");

                entity.Property(e => e.InviteCode).HasColumnName("invite_code");

                entity.Property(e => e.ChallengedTo).HasColumnName("challenged_to");

                entity.Property(e => e.ChallengerId).HasColumnName("challenger_id");

                entity.Property(e => e.ImageUrl)
                    .IsUnicode(false)
                    .HasColumnName("image_url");

                entity.HasOne(d => d.ChallengedToNavigation)
                    .WithMany(p => p.ChallengeChallengedToNavigations)
                    .HasForeignKey(d => d.ChallengedTo)
                    .HasConstraintName("FK__challenge__chall__03F0984C");

                entity.HasOne(d => d.Challenger)
                    .WithMany(p => p.ChallengeChallengers)
                    .HasForeignKey(d => d.ChallengerId)
                    .HasConstraintName("FK__challenge__chall__02FC7413");
            });

            modelBuilder.Entity<Destination>(entity =>
            {
                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CorrectAnswers)
                    .HasColumnName("correct_answers")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IncorrectAnswers)
                    .HasColumnName("incorrect_answers")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Score).HasDefaultValueSql("((0))");

                entity.Property(e => e.Username).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
