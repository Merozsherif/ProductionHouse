using Microsoft.EntityFrameworkCore;
using ProductionHouse.Core.Entities; // استدعاء الـ Entities من الـ Core

namespace ProductionHouse.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 1) تعريف الجداول في قاعدة البيانات (DbSets)
        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
        public DbSet<ProjectTranslation> ProjectTranslations { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Admin> Admins { get; set; }
        // 2) رسم العلاقات بالتفصيل باستخدام الـ Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // سطر أساسي ومهم جداً لتأمين الـ Identity والـ Base Configurations
            base.OnModelCreating(modelBuilder);

            // علاقة المشروع مع القسم (كل قسم له مشاريع كثيرة، والمشروع يتبع قسم واحد)
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // حماية البيانات من الحذف العشوائي

            // علاقة المشروع مع الصور (المشروع له صور كثيرة، والصورة تتبع مشروع واحد)
            modelBuilder.Entity<ProjectImage>()
                .HasOne(i => i.Project)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); // لو مسحنا مشروع، صوره تتمسح تلقائياً

            // علاقة الترجمة مع المشروع (دعم اللغات)
            modelBuilder.Entity<ProjectTranslation>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Translations)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // علاقة ترجمة التصنيفات والأقسام
            modelBuilder.Entity<CategoryTranslation>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Translations)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}