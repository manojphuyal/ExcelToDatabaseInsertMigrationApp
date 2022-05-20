using System;
using System.Collections.Generic;
using System.Text;
using FileApp.Localization;
using Volo.Abp.Application.Services;

namespace FileApp
{
    /* Inherit your application services from this class.
     */
    public abstract class FileAppAppService : ApplicationService
    {
        protected FileAppAppService()
        {
            LocalizationResource = typeof(FileAppResource);
        }
    }
}
