using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileApp.CqNovelImport.ImportContact
{
    public interface IImportContactAppService
    {
        #region 1 Import ContactCompany  
        Task<string> ImportExcelContactCompany([FromForm] IFormFile excel);
        #endregion
        #region 2 Import ContactPerson     
        Task<string> ImportExcelContactPerson([FromForm] IFormFile excel);
        #endregion
        #region 3 Import ContactLabelData     
        Task<string> ImportExcelContactLabelData([FromForm] IFormFile excel);
        #endregion
        #region 4 Import ContactAddress     
        Task<string> ImportExcelContactAddress([FromForm] IFormFile excel);
        #endregion
        #region 5 Import Country     
        Task<string> ImportExcelCountry([FromForm] IFormFile excel);
        #endregion
        #region 6 Import City     
        Task<string> ImportExcelCity([FromForm] IFormFile excel);
        #endregion

    }
}
