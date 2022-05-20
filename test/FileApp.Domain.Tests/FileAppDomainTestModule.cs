using FileApp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace FileApp
{
    [DependsOn(
        typeof(FileAppEntityFrameworkCoreTestModule)
        )]
    public class FileAppDomainTestModule : AbpModule
    {

    }
}