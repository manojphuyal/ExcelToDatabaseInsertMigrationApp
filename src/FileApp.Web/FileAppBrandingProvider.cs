using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace FileApp.Web
{
    [Dependency(ReplaceServices = true)]
    public class FileAppBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "FileApp";
    }
}
