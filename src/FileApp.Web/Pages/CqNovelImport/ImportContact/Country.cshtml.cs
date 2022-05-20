using FileApp.CqNovelImport.ImportContact;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FileApp.Web.Pages.CqNovelImport.ImportContact
{
    public class CountryModel : FileAppPageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please upload excel file")]
        [FileExtensions(Extensions = ".xlsx", ErrorMessage = "Valid Only Excel File")]
        public IFormFile ExcelFile { get; set; }
        [BindProperty]
        public string Message { get; set; }
        [BindProperty]
        public string Code { get; set; }

        private readonly IImportContactAppService _countryRepository;
        public CountryModel(IImportContactAppService countryRepository)
        {
            _countryRepository = countryRepository;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ExcelFile != null)
                {
                    await _countryRepository.ImportExcelCountry(ExcelFile);
                    return RedirectToPage("/", new { Message = "Successfully Inserted!", Code = "1" });
                }
                else
                {
                    return RedirectToPage("/", new { Message = "No file selected!", Code = "2" });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}