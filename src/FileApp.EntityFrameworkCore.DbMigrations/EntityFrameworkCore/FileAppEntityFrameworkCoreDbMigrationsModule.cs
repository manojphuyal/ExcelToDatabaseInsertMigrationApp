using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace FileApp.EntityFrameworkCore
{
    [DependsOn(
        typeof(FileAppEntityFrameworkCoreModule)
        )]
    public class FileAppEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<FileAppMigrationsDbContext>();
        }
    }
}
