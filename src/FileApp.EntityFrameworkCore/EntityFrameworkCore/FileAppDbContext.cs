using Microsoft.EntityFrameworkCore;
using FileApp.Users;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;
using Volo.Abp.Users.EntityFrameworkCore;
using FileApp.AppEntities;
using FileApp.AppEntities.CqNovelImport.ImportContact;
using FileApp.AppEntities.CqNovelImport.OldImportContact;
using FileApp.AppEntities.CqNovelImport.ImportContact.SystemTable;

namespace FileApp.EntityFrameworkCore
{
    /* This is your actual DbContext used on runtime.
     * It includes only your entities.
     * It does not include entities of the used modules, because each module has already
     * its own DbContext class. If you want to share some database tables with the used modules,
     * just create a structure like done for AppUser.
     *
     * Don't use this DbContext for database migrations since it does not contain tables of the
     * used modules (as explained above). See FileAppMigrationsDbContext for migrations.
     */
    [ConnectionStringName("Default")]
    public class FileAppDbContext : AbpDbContext<FileAppDbContext>
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<NameModel> NameModels { get; set; }


        public DbSet<Reference> References { get; set; }
        //public DbSet<ReferenceType> ReferenceType { get; set; }
        public DbSet<ReferenceType> ReferenceTypes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public  DbSet<TenderBasic> TenderBasics { get; set; }
        public  DbSet<ReferenceOthers> ReferenceOthers { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<ContactLabel> ContactLabels { get; set; }

        public  DbSet<ContactAddress> ContactAddresses { get; set; }
        public  DbSet<ContactCompany> ContactCompanies { get; set; }
        public  DbSet<ContactLabelData> ContactLabelDatas { get; set; }
        public  DbSet<ContactPerson> ContactPeople { get; set; }
        public  DbSet<AttendanceDetail> AttendanceDetails { get; set; }










        /* Add DbSet properties for your Aggregate Roots / Entities here.
         * Also map them inside FileAppDbContextModelCreatingExtensions.ConfigureFileApp
         */

        public FileAppDbContext(DbContextOptions<FileAppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Configure the shared tables (with included modules) here */

            builder.Entity<AppUser>(b =>
            {
                b.ToTable(AbpIdentityDbProperties.DbTablePrefix + "Users"); //Sharing the same table "AbpUsers" with the IdentityUser
                
                b.ConfigureByConvention();
                b.ConfigureAbpUser();

                /* Configure mappings for your additional properties
                 * Also see the FileAppEfCoreEntityExtensionMappings class
                 */
            });

            /* Configure your own tables/entities inside the ConfigureFileApp method */

            builder.ConfigureFileApp();
        }
    }
}
