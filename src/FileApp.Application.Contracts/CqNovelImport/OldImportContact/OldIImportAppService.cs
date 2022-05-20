

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileApp.CqNovelImport.OldImportContact
{
    public interface IImportOldContactAppService
    {
        #region 1 Import ContactCompany  
        Task<string> ImportExcelOldContactCompany([FromForm] IFormFile excel);
        #endregion
    }
}
