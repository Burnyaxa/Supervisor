using System;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Supervisor
{
    public partial class DreamContext : DbContext
    {
        public DreamContext()
        {
        }

        public DreamContext(DbContextOptions<DreamContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Keyword> Keywords { get; set; }
        public virtual DbSet<KeywordsHistory> KeywordsHistories { get; set; }
        public virtual DbSet<ObservedKeyword> ObservedKeywords { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserKeyword> UserKeywords { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("dream-context-connection-string",
                    EnvironmentVariableTarget.User));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "C.UTF-8");

            modelBuilder.Entity<Keyword>(entity =>
            {
                entity.ToTable("keywords");

                entity.HasIndex(e => e.Keyword1, "keywords_keyword_uindex")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Keyword1)
                    .IsRequired()
                    .HasColumnName("keyword");
            });

            modelBuilder.Entity<KeywordsHistory>(entity =>
            {
                entity.ToTable("keywords_history");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt).HasColumnName("created_at");

                entity.Property(e => e.Data)
                    .IsRequired()
                    .HasColumnType("jsonb")
                    .HasColumnName("data");

                entity.Property(e => e.KeywordId).HasColumnName("keyword_id");

                entity.HasOne(d => d.Keyword)
                    .WithMany(p => p.KeywordsHistories)
                    .HasForeignKey(d => d.KeywordId)
                    .HasConstraintName("keywords_history_keywords_fk");
            });

            modelBuilder.Entity<ObservedKeyword>(entity =>
            {
                entity.HasKey(e => e.KeywordId)
                    .HasName("observed_keywords_pk");

                entity.ToTable("observed_keywords");

                entity.HasIndex(e => e.KeywordId, "observed_keywords_keyword_id_uindex")
                    .IsUnique();

                entity.Property(e => e.KeywordId)
                    .ValueGeneratedNever()
                    .HasColumnName("keyword_id");

                entity.Property(e => e.AddedAt)
                    .HasColumnName("added_at")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Keyword)
                    .WithOne(p => p.ObservedKeyword)
                    .HasForeignKey<ObservedKeyword>(d => d.KeywordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("observed_keywords_keyword_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Email, "users_email_uindex")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("nextval('user_id_seq'::regclass)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Iv)
                    .IsRequired()
                    .HasColumnName("iv");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password");

                entity.Property(e => e.PasswordVersion)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("password_version");
            });

            modelBuilder.Entity<UserKeyword>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("user_keyword");

                entity.Property(e => e.KeywordId).HasColumnName("keyword_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Keyword)
                    .WithMany()
                    .HasForeignKey(d => d.KeywordId)
                    .HasConstraintName("user_keyword_keywords_id_fk");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("user_keyword_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}