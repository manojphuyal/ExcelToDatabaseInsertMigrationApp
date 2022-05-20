﻿using FileApp.CqNovelImport.ImportContact;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FileApp.Web.Pages.CqNovelImport.ImportContact
{
    public class CityModel : FileAppPageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please upload excel file")]
        [FileExtensions(Extensions = ".xlsx", ErrorMessage = "Valid Only Excel File")]
        public IFormFile ExcelFile { get; set; }
        [BindProperty]
        public string Message { get; set; }
        [BindProperty]
        public string Code { get; set; }

        private readonly IImportContactAppService _cityRepository;
        public CityModel(IImportContactAppService cityRepository)
        {
            _cityRepository = cityRepository;
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
                    await _cityRepository.ImportExcelCity(ExcelFile);
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