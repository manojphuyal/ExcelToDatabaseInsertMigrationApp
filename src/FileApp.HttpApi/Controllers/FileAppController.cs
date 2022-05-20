using FileApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace FileApp.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class FileAppController : AbpController
    {
        protected FileAppController()
        {
            LocalizationResource = typeof(FileAppResource);
        }
    }
}