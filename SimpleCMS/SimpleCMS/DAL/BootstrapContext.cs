using SimpleCMS.Models;
using SimpleCMS.Models.Blog;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SimpleCMS.DAL
{
    //[DbConfigurationType(typeof(BootstrapConfiguration))]
    public partial class BootstrapContext : DbContext
    {
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Permalink> Permalinks { get; set; }
        public virtual DbSet<Plugin> Plugins { get; set; }
        public virtual DbSet<PluginFile> PluginFiles { get; set; }
        public virtual DbSet<PageContent> PageContents { get; set; }
        public virtual DbSet<PageContentFile> PageContentFiles { get; set; }
        public virtual DbSet<BlogWebsiteSettings> BlogWebsiteSettings { get; set; }
        public virtual DbSet<ViewTemplate> ViewTemplates { get; set; }
        public virtual DbSet<ImageInfo> Images { get; set; }
        public virtual DbSet<BlogItemContentData> BlogItemContentDatas { get; set; }
        public virtual DbSet<PageContentInfo> PageContentInfos { get; set; }
        public virtual DbSet<BlogItemContent> BlogItemContents { get; set; }
        public virtual DbSet<PageContentType> PageContentTypes { get; set; }
        public virtual DbSet<ContentTranslation> ContentTranslations { get; set; }
        public virtual DbSet<BlogFeedSettings> BlogFeedSettings { get; set; }
        public virtual DbSet<ViewTemplateFile> ViewTemplateFiles { get; set; }
        public virtual DbSet<ImageContentData> ImageContentDatas { get; set; }
        public virtual DbSet<ImageContent> ImageContents { get; set; }
        public virtual DbSet<PageItemContentData> PageItemContentDatas { get; set; }
        public virtual DbSet<PageItemContent> PageItemContents { get; set; }
        public virtual DbSet<ChangeEvent> ChangeEvents { get; set; }

        public BootstrapContext()
            : base("name=SimpleCMSContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BootstrapContext, Migrations.DALConfiguration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Properties<decimal>().Configure(c => c.HasPrecision(18, 4));
            base.OnModelCreating(modelBuilder);
        }
    }
}