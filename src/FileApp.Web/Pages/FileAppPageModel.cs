using FileApp.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace FileApp.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class FileAppPageModel : AbpPageModel
    {
        protected FileAppPageModel()
        {
            LocalizationResourceType = typeof(FileAppResource);
        }
    }
}