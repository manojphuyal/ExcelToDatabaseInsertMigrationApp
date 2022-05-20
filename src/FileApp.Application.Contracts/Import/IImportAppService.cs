using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileApp.Import
{
    public interface IImportAppService
    {
        #region Import Reference
        Task<string> ImportExcelReference([FromForm] IFormFile excel);
        #endregion        
        #region Import Project     
        Task<string> ImportExcelProject([FromForm] IFormFile excel);
        #endregion
        #region Import Tender     
        Task<string> ImportExcelTender([FromForm] IFormFile excel);
        #endregion
        #region Import Other     
        Task<string> ImportExcelOther([FromForm] IFormFile excel);
        #endregion

        Task<string> ImportExcelAttendance([FromForm] IFormFile excel);

    }
}
