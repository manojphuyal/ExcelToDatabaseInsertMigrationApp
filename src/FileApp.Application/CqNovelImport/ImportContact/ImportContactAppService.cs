using FileApp.AppEntities.CqNovelImport.ImportContact;
using FileApp.AppEntities.CqNovelImport.ImportContact.SystemTable;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace FileApp.CqNovelImport.ImportContact
{
    public class OldImportContactAppService : ApplicationService, IImportContactAppService
    {
        private readonly IRepository<ContactCompany, Guid> _contactCompanyRepository;
        private readonly IRepository<ContactPerson, Guid> _contactPersonRepository;
        private readonly IRepository<ContactLabelData, Guid> _contactLabelDataRepository;
        private readonly IRepository<ContactLabel, Guid> _contactLabelRepository;
        private readonly IRepository<ContactAddress, Guid> _contactAddressRepository;


        //private readonly IRepository<Gender, Guid> _genderRepository;
        //private readonly IRepository<MaritalStatus, Guid> _maritalStatusRepository;
        private readonly IRepository<City, Guid> _cityRepository;
        private readonly IRepository<Country, Guid> _countryRepository;

        public OldImportContactAppService(IRepository<ContactCompany, Guid> contactCompanyRepository,
                                    IRepository<ContactPerson, Guid> contactPersonRepository,
                                    IRepository<ContactLabelData, Guid> contactLabelDataRepository,
                                    IRepository<ContactLabel, Guid> contactLabelRepository,
                                    IRepository<ContactAddress, Guid> contactAddressRepository,

                                    //IRepository<Gender, Guid> genderRepository,
                                    //IRepository<MaritalStatus, Guid> maritalStatusRepository,
                                    IRepository<City, Guid> cityRepository,
                                    IRepository<Country, Guid> countryRepository
                                    )
        {
            this._contactCompanyRepository = contactCompanyRepository;
            this._contactPersonRepository = contactPersonRepository;
            this._contactLabelDataRepository = contactLabelDataRepository;
            this._contactLabelRepository = contactLabelRepository;
            this._contactAddressRepository = contactAddressRepository;

            //this._genderRepository = genderRepository;
            //this._maritalStatusRepository = maritalStatusRepository;
            this._cityRepository = cityRepository;
            this._countryRepository = countryRepository;
        }
        #region 1 Import ContactCompany
        [HttpPost]
        public async Task<string> ImportExcelContactCompany([FromForm] IFormFile excel)
        {
            try
            {
                if (excel == null | excel.Length <= 0)
                {
                    throw new UserFriendlyException("File not found");
                }
                if (!Path.GetExtension(excel.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    throw new UserFriendlyException("File fotmat not correct");
                }
                var list = new List<ContactCompany>();

                using (var stream = new MemoryStream())
                {
                    await excel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        var excelColoumn = new Dictionary<string, int>() {
                            {"oldid",0},
                            {"companyname",0},
                            {"companyabbrevation",0},
                            {"defaultthrough",0},
                            { "notes",0},
                            { "scopeofwork",0},
                            { "mailaddress",0},
                            { "companyceo",0},
                        };

                        //for(var i in worksheet.Column)
                        var coloumnCount = worksheet.Dimension.End.Column;
                        for (int i = 1; i <= coloumnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("companyname", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["companyname"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("companyabbrevation", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["companyabbrevation"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("defaultthrough", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["defaultthrough"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("notes", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["notes"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("scopeofwork", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["scopeofwork"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("mailaddress", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["mailaddress"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("companyceo", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["companyceo"] = i;
                            }
                        }


                        var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("oldid", StringComparison.OrdinalIgnoreCase);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var Oldid = worksheet.Cells[row, excelColoumn["oldid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldid"]].Value.ToString().Trim();

                            var CompanyName = worksheet.Cells[row, excelColoumn["companyname"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["companyname"]].Value.ToString().Trim();

                            var CompanyAbbrevation = worksheet.Cells[row, excelColoumn["companyabbrevation"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["companyabbrevation"]].Value.ToString().Trim();

                            var DefaultThrough = worksheet.Cells[row, excelColoumn["defaultthrough"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["defaultthrough"]].Value.ToString().Trim();

                            var Notes = worksheet.Cells[row, excelColoumn["notes"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["notes"]].Value.ToString().Trim();

                            var ScopeOfWork = worksheet.Cells[row, excelColoumn["scopeofwork"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["scopeofwork"]].Value.ToString().Trim();
                            var MailAddress = worksheet.Cells[row, excelColoumn["mailaddress"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["mailaddress"]].Value.ToString().Trim();

                            var CompanyCEO = worksheet.Cells[row, excelColoumn["companyceo"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["companyceo"]].Value.ToString().Trim();

                            list.Add(new ContactCompany
                            {
                                OldId= int.Parse(Oldid),
                                CompanyName = CompanyName,
                                CompanyAbbrevation = CompanyAbbrevation,
                                ParentCompanyId = null,
                                ContactSourceId = Guid.Parse("8F9224F5-8FB0-4366-9305-0A92EFFE3101"),
                                DefaultThrough = DefaultThrough,
                                Notes = Notes,
                                ScopeOfWork = ScopeOfWork,
                                MailAddress = MailAddress,
                                CompanyCEO = CompanyCEO,
                                IsQuickEntry = false,
                                TagNames = "",
                                IsActive = true
                            });
                        }
                    }
                }
                if (list.Count > 0)
                {
                    await _contactCompanyRepository.InsertManyAsync(list);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return "Sucessfully Uploaded in database";
                }

                return "Excel file couldnot be uploaded";
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        #endregion Import

        #region 2 Import ContactPerson
        [HttpPost]
        public async Task<string> ImportExcelContactPerson([FromForm] IFormFile excel)
        {
            try
            {
                if (excel == null | excel.Length <= 0)
                {
                    throw new UserFriendlyException("File not found");
                }
                if (!Path.GetExtension(excel.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    throw new UserFriendlyException("File fotmat not correct");
                }
                var list = new List<ContactPerson>();

                using (var stream = new MemoryStream())
                {
                    await excel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        var excelColoumn = new Dictionary<string, int>() {
                            {"oldid",0},
                            {"oldcityid",0},
                            {"oldcountryid",0},
                            {"suffix",0},
                            {"firstname",0},
                            {"middlename",0},
                            { "lastname",0},
                            { "nickname",0},
                            { "personalabbrevation",0},
                            { "dateofbirth",0},
                            { "anniversarydate",0},
                            { "spousename",0},
                            { "departmentname",0},
                            { "hobbies",0},
                            { "notes",0},
                            { "genderid",0},
                            { "maritalstatusid",0},
                            { "companycontactid",0},
                            { "defaultthrough",0},
                        };

                        //for(var i in worksheet.Column)
                        var coloumnCount = worksheet.Dimension.End.Column;
                        for (int i = 1; i <= coloumnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldcityid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldcityid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldcountryid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldcountryid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("suffix", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["suffix"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("firstname", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["firstname"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("middlename", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["middlename"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("lastname", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["lastname"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("nickname", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["nickname"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("personalabbrevation", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["personalabbrevation"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("dateofbirth", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["dateofbirth"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("anniversarydate", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["anniversarydate"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("spousename", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["spousename"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("departmentname", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["departmentname"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("hobbies", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["hobbies"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("notes", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["notes"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("genderid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["genderid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("maritalstatusid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["maritalstatusid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("companycontactid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["companycontactid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("defaultthrough", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["defaultthrough"] = i;
                            }
                        }


                        var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("oldid", StringComparison.OrdinalIgnoreCase);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var OldId = worksheet.Cells[row, excelColoumn["oldid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldid"]].Value.ToString().Trim();

                            var OldCityId = worksheet.Cells[row, excelColoumn["oldcityid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldcityid"]].Value.ToString().Trim();

                            var OldCountryId = worksheet.Cells[row, excelColoumn["oldcountryid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldcountryid"]].Value.ToString().Trim();

                            var Suffix = worksheet.Cells[row, excelColoumn["suffix"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["suffix"]].Value.ToString().Trim();

                            var FirstName = worksheet.Cells[row, excelColoumn["firstname"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["firstname"]].Value.ToString().Trim();

                            var MiddleName = worksheet.Cells[row, excelColoumn["middlename"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["middlename"]].Value.ToString().Trim();

                            var LastName = worksheet.Cells[row, excelColoumn["lastname"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["lastname"]].Value.ToString().Trim();

                            var NickName = worksheet.Cells[row, excelColoumn["nickname"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["nickname"]].Value.ToString().Trim();
                            var PersonalAbbrevation = worksheet.Cells[row, excelColoumn["personalabbrevation"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["personalabbrevation"]].Value.ToString().Trim();
                            var DateOfBirth = worksheet.Cells[row, excelColoumn["dateofbirth"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["dateofbirth"]].Text.Trim();

                            var AnniversaryDate = worksheet.Cells[row, excelColoumn["anniversarydate"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["anniversarydate"]].Text.Trim();

                            var SpouseName = worksheet.Cells[row, excelColoumn["spousename"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["spousename"]].Value.ToString().Trim();

                            var DepartmentName = worksheet.Cells[row, excelColoumn["departmentname"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["departmentname"]].Value.ToString().Trim();

                            var Hobbies = worksheet.Cells[row, excelColoumn["hobbies"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["hobbies"]].Value.ToString().Trim();

                            var Notes = worksheet.Cells[row, excelColoumn["notes"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["notes"]].Value.ToString().Trim();

                            var GId = worksheet.Cells[row, excelColoumn["genderid"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["genderid"]].Value.ToString().Trim();

                            var MsId = worksheet.Cells[row, excelColoumn["maritalstatusid"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["maritalstatusid"]].Value.ToString().Trim();


                            var Companyid = worksheet.Cells[row, excelColoumn["companycontactid"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["companycontactid"]].Value.ToString().Trim();


                            var DefaultThrough = worksheet.Cells[row, excelColoumn["defaultthrough"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["defaultthrough"]].Value.ToString().Trim();

                            var GenderId = GenderIdGetDetails(GId).Result;
                            var MaritalStatusId = MaritalStatusGetDetails(MsId).Result;
                            Guid CompanyContactId = CompanyContactIdGetDetails(Companyid).Result;
                            list.Add(new ContactPerson
                            {
                                OldId  = OldId,
                                OldCityId = OldCityId,
                                OldCountryId = OldCountryId,
                                Suffix = Suffix,
                                FirstName = FirstName,
                                MiddleName = MiddleName,
                                LastName = LastName,
                                NickName = NickName,
                                PersonalAbbrevation = PersonalAbbrevation,
                                //DateOfBirth = DateTime.Parse(DateOfBirth),
                                //AnniversaryDate = DateTime.Parse(AnniversaryDate),
                                SpouseName = SpouseName,
                                DesignationName = "",
                                DepartmentName = DepartmentName,
                                Hobbies = Hobbies,
                                Notes = Notes,
                                GenderId = GenderId,
                                MaritalStatusId = MaritalStatusId,
                                CompanyContactId = CompanyContactId,
                                ConatactSourceId = Guid.Parse("8F9224F5-8FB0-4366-9305-0A92EFFE3101"),
                                DefaultThrough = DefaultThrough,
                                IsQuickEntry = false,
                                TagNames = "",
                                IsActive = true
                            });
                        }
                    }
                }
                if (list.Count > 0)
                {
                    await _contactPersonRepository.InsertManyAsync(list);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return "Sucessfully Uploaded in database";
                }

                return "Excel file couldnot be uploaded";
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        [HttpGet]
        public async Task<Guid> MaritalStatusGetDetails(string marritalId)
        {
            try
            {
                if (marritalId == "Married")
                {
                    return Guid.Parse("D4A75E32-99FA-48B7-8850-EA888E8ECDAB");
                }
                else if (marritalId == "UnMarried")
                {
                    return Guid.Parse("2DB7EAB2-EF0D-492A-B369-50FBE8E7D574");
                }
                else
                {
                    return Guid.Parse("2DB7EAB2-EF0D-492A-B369-50FBE8E7D574");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<Guid> GenderIdGetDetails(string genderId)
        {
            try
            {
                if (genderId == "1")
                {
                    return Guid.Parse("09AEA3E2-94F8-4A2E-ABBB-A33AAF28E3CF");
                }
                else if (genderId == "2")
                {
                    return Guid.Parse("D0A95E65-A202-4245-A3EE-B658991F4F6A");
                }
                else
                {
                    return Guid.Parse("5D1A26BA-7868-4EF6-9895-B5883612D84F");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<Guid> CompanyContactIdGetDetails(string Companyid)
        {
            var CompanyName = _contactCompanyRepository.Where(x => x.OldId.Equals(int.Parse(Companyid))).FirstOrDefault();
            try
            {
                if (CompanyName is null)
                {
                    return Guid.Parse("00000000-0000-0000-0000-000000000000");
                }
                else
                {
                    return Guid.Parse(CompanyName.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion Import

        #region 3 Import ContactLabelData
        [HttpPost]
        public async Task<string> ImportExcelContactLabelData([FromForm] IFormFile excel)
        {
            try
            {
                if (excel == null | excel.Length <= 0)
                {
                    throw new UserFriendlyException("File not found");
                }
                if (!Path.GetExtension(excel.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    throw new UserFriendlyException("File fotmat not correct");
                }
                var list = new List<ContactLabelData>();

                using (var stream = new MemoryStream())
                {
                    await excel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        var excelColoumn = new Dictionary<string, int>() {
                            {"oldid",0},
                            {"labeldata",0},
                            {"contactlabelid",0},
                            { "contactpersonid",0},
                            { "contactcompanyid",0},
                        };

                        //for(var i in worksheet.Column)
                        var coloumnCount = worksheet.Dimension.End.Column;
                        for (int i = 1; i <= coloumnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("labeldata", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["labeldata"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("contactlabelid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["contactlabelid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("contactpersonid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["contactpersonid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("contactcompanyid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["contactcompanyid"] = i;
                            }
                        }


                        var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("oldid", StringComparison.OrdinalIgnoreCase);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var OldId = worksheet.Cells[row, excelColoumn["oldid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldid"]].Value.ToString().Trim();

                            var LabelData = worksheet.Cells[row, excelColoumn["labeldata"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["labeldata"]].Value.ToString().Trim();

                            var ContactLabelId = worksheet.Cells[row, excelColoumn["contactlabelid"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["contactlabelid"]].Value.ToString().Trim();

                            var ContactPersonId = worksheet.Cells[row, excelColoumn["contactpersonid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["contactpersonid"]].Value.ToString().Trim();

                            var ContactCompanyId = worksheet.Cells[row, excelColoumn["contactcompanyid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["contactcompanyid"]].Value.ToString().Trim();

                            var contactLId = ContactLabeGetDetails(ContactLabelId).Result;
                            var contactPLId = ContactPersonGetDetails(ContactPersonId).Result;
                            var contactCId = ContactCompanyGetDetails(ContactCompanyId).Result;
                            list.Add(new ContactLabelData
                            {
                                OldId = OldId,
                                LabelData = LabelData,
                                ContactLabelId = contactLId,
                                ContactPersonId = contactPLId,
                                ContactCompanyId = contactCId,
                                IsActive=true
                            });
                        }
                    }
                }
                if (list.Count > 0)
                {
                    await _contactLabelDataRepository.InsertManyAsync(list);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return "Sucessfully Uploaded in database";
                }

                return "Excel file couldnot be uploaded";
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        [HttpGet]
        public async Task<Guid> ContactLabeGetDetails(string ContactLabelId)
        {
            var data = _contactLabelRepository.Where(x => x.OldId.Equals(ContactLabelId)).FirstOrDefault();
            try
            {
                if (data is null)
                {
                    return Guid.Parse("00000000-0000-0000-0000-000000000000");
                }
                else
                {
                    return Guid.Parse(data.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<Guid> ContactPersonGetDetails(string ContactPersonId)
        {
            var data = _contactPersonRepository.Where(x => x.OldId.Equals(ContactPersonId)).FirstOrDefault();
            try
            {
                if (data is null)
                {
                    return Guid.Parse("00000000-0000-0000-0000-000000000000");
                }
                else
                {
                    return Guid.Parse(data.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<Guid> ContactCompanyGetDetails(string ContactCompanyId)
        {
            var data = _contactCompanyRepository.Where(x => x.OldId.Equals(int.Parse(ContactCompanyId))).FirstOrDefault();
            try
            {
                if (data is null)
                {
                    return Guid.Parse("00000000-0000-0000-0000-000000000000");
                }
                else
                {
                    return Guid.Parse(data.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion Import

        #region 4 Import ContactAddress
        [HttpPost]
        public async Task<string> ImportExcelContactAddress([FromForm] IFormFile excel)
        {
            try
            {
                if (excel == null | excel.Length <= 0)
                {
                    throw new UserFriendlyException("File not found");
                }
                if (!Path.GetExtension(excel.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    throw new UserFriendlyException("File fotmat not correct");
                }
                var list = new List<ContactAddress>();

                using (var stream = new MemoryStream())
                {
                    await excel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        var excelColoumn = new Dictionary<string, int>() {
                            {"oldid",0},
                            {"contactpersonid",0},
                            {"contactcompanyid",0},
                            {"contactlabelid",0},
                            {"countryid",0},
                            { "cityid",0},
                            { "streetname",0},
                            { "pobox",0},
                            { "stateprovince",0},
                            { "postalcodezip",0},
                            { "isprimaryaddress",0},
                        };

                        //for(var i in worksheet.Column)
                        var coloumnCount = worksheet.Dimension.End.Column;
                        for (int i = 1; i <= coloumnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("contactpersonid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["contactpersonid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("contactcompanyid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["contactcompanyid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("contactlabelid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["contactlabelid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("countryid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["countryid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("cityid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["cityid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("streetname", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["streetname"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("pobox", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["pobox"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("stateprovince", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["stateprovince"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("postalcodezip", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["postalcodezip"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("isprimaryaddress", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["isprimaryaddress"] = i;
                            }

                        }


                        var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("oldid", StringComparison.OrdinalIgnoreCase);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var OldId = worksheet.Cells[row, excelColoumn["oldid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldid"]].Value.ToString().Trim();

                            var ContactPersonId = worksheet.Cells[row, excelColoumn["contactpersonid"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["contactpersonid"]].Value.ToString().Trim();

                            var ContactCompanyId = worksheet.Cells[row, excelColoumn["contactcompanyid"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["contactcompanyid"]].Value.ToString().Trim();

                            var ContactLabelId = worksheet.Cells[row, excelColoumn["contactlabelid"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["contactlabelid"]].Value.ToString().Trim();

                            var CountryId = worksheet.Cells[row, excelColoumn["countryid"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["countryid"]].Value.ToString().Trim();

                            var CityId = worksheet.Cells[row, excelColoumn["cityid"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["cityid"]].Value.ToString().Trim();

                            var StreetName = worksheet.Cells[row, excelColoumn["streetname"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["streetname"]].Value.ToString().Trim();

                            var PoBox = worksheet.Cells[row, excelColoumn["pobox"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["pobox"]].Value.ToString().Trim();

                            var StateProvince = worksheet.Cells[row, excelColoumn["stateprovince"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["stateprovince"]].Value.ToString().Trim();

                            var PostalCodeZip = worksheet.Cells[row, excelColoumn["postalcodezip"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["postalcodezip"]].Value.ToString().Trim();

                            var IsPrimaryAddress = worksheet.Cells[row, excelColoumn["isprimaryaddress"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["isprimaryaddress"]].Value.ToString().Trim();

                            var contactPerLId = ContactPersonGetDetails(ContactPersonId).Result;
                            var contactCompId = ContactCompanyGetDetails(ContactCompanyId).Result;
                            var contactLabId = ContactLabeGetDetails(ContactLabelId).Result;
                            var contactCouId = CountryGetDetails(CountryId).Result;
                            var contactCitId = CityGetDetails(CityId).Result;
                            list.Add(new ContactAddress
                            {
                                OldId = OldId,
                                ContactPersonId = contactPerLId,
                                ContactCompanyId = contactCompId,
                                ContactLabelId = contactLabId,
                                CountryId = contactCouId,
                                CityId = contactCitId,
                                StreetName = StreetName,
                                PoBox = PoBox,
                                StateProvince = StateProvince,
                                PostalCodeZip = PostalCodeZip,
                                IsPrimaryAddress = bool.Parse(IsPrimaryAddress),
                                IsActive = true,

                            });
                        }
                    }
                }
                if (list.Count > 0)
                {
                    await _contactAddressRepository.InsertManyAsync(list);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return "Sucessfully Uploaded in database";
                }

                return "Excel file couldnot be uploaded";
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        [HttpGet]
        public async Task<Guid> CountryGetDetails(string CountryId)
        {
            var data = _countryRepository.Where(x => x.OldId.Equals(CountryId)).FirstOrDefault();
            try
            {
                if (data is null)
                {
                    return Guid.Parse("00000000-0000-0000-0000-000000000000");
                }
                else
                {
                    return Guid.Parse(data.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<Guid> CityGetDetails(string CityId)
        {
            var data = _cityRepository.Where(x => x.OldId.Equals(CityId)).FirstOrDefault();
            try
            {
                if (data is null)
                {
                    return Guid.Parse("00000000-0000-0000-0000-000000000000");
                }
                else
                {
                    return Guid.Parse(data.Id.ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion Import

        #region 4 Import Country
        [HttpPost]
        public async Task<string> ImportExcelCountry([FromForm] IFormFile excel)
        {
            try
            {
                if (excel == null | excel.Length <= 0)
                {
                    throw new UserFriendlyException("File not found");
                }
                if (!Path.GetExtension(excel.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    throw new UserFriendlyException("File fotmat not correct");
                }
                var list = new List<Country>();

                using (var stream = new MemoryStream())
                {
                    await excel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        var excelColoumn = new Dictionary<string, int>() {
                            {"oldid",0},
                            {"code",0},
                            {"name",0},
                        };

                        //for(var i in worksheet.Column)
                        var coloumnCount = worksheet.Dimension.End.Column;
                        for (int i = 1; i <= coloumnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("code", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["code"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("name", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["name"] = i;
                            }                         

                        }


                        var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("oldid", StringComparison.OrdinalIgnoreCase);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var OldId = worksheet.Cells[row, excelColoumn["oldid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldid"]].Value.ToString().Trim();

                            var Code = worksheet.Cells[row, excelColoumn["code"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["code"]].Value.ToString().Trim();

                            var Name = worksheet.Cells[row, excelColoumn["name"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["name"]].Value.ToString().Trim();

                            list.Add(new Country
                            {
                                OldId = OldId,
                                Code = Code,
                                Name = Name,
                                IsActive = true,

                            });
                        }
                    }
                }
                if (list.Count > 0)
                {
                    await _countryRepository.InsertManyAsync(list);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return "Sucessfully Uploaded in database";
                }

                return "Excel file couldnot be uploaded";
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        #endregion Import
        #region 4 Import City
        [HttpPost]
        public async Task<string> ImportExcelCity([FromForm] IFormFile excel)
        {
            try
            {
                if (excel == null | excel.Length <= 0)
                {
                    throw new UserFriendlyException("File not found");
                }
                if (!Path.GetExtension(excel.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    throw new UserFriendlyException("File fotmat not correct");
                }
                var list = new List<City>();

                using (var stream = new MemoryStream())
                {
                    await excel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        var excelColoumn = new Dictionary<string, int>() {
                            {"oldid",0},
                            {"name",0},
                            {"countryid",0},
                        };

                        //for(var i in worksheet.Column)
                        var coloumnCount = worksheet.Dimension.End.Column;
                        for (int i = 1; i <= coloumnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("name", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["name"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("countryid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["countryid"] = i;
                            }

                        }


                        var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("oldid", StringComparison.OrdinalIgnoreCase);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var OldId = worksheet.Cells[row, excelColoumn["oldid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldid"]].Value.ToString().Trim();

                            var Name = worksheet.Cells[row, excelColoumn["name"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["name"]].Value.ToString().Trim();

                            var CountryId = worksheet.Cells[row, excelColoumn["countryid"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["countryid"]].Value.ToString().Trim();

                            var cId = CountryGetDetails(CountryId).Result;
                            list.Add(new City
                            {
                                OldId = OldId,
                                Name = Name,
                                CountryId = cId,
                                IsActive = true,

                            });
                        }
                    }
                }
                if (list.Count > 0)
                {
                    await _cityRepository.InsertManyAsync(list);
                    await CurrentUnitOfWork.SaveChangesAsync();
                    return "Sucessfully Uploaded in database";
                }

                return "Excel file couldnot be uploaded";
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }
        #endregion Import


    }
}


