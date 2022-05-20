using FileApp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace FileApp.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(FileAppEntityFrameworkCoreDbMigrationsModule),
        typeof(FileAppApplicationContractsModule)
        )]
    public class FileAppDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
