using Volo.Abp.Modularity;

namespace FileApp
{
    [DependsOn(
        typeof(FileAppApplicationModule),
        typeof(FileAppDomainTestModule)
        )]
    public class FileAppApplicationTestModule : AbpModule
    {

    }
}