using FileApp.AppEntities;
using FileApp.AppEntities.CqNovelImport.ImportContact;
using FileApp.AppEntities.CqNovelImport.OldImportContact;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace FileApp.Import
{
    public class ImportAppService : ApplicationService, IImportAppService
    {
            private readonly IRepository<Reference, Guid> _referenceModelRepository;
            private readonly IRepository<ReferenceType, Guid> _referenceTypeModelRepository;
            private readonly IRepository<TenderBasic, Guid> _tenderBasicRepository;
            private readonly IRepository<Project, Guid> _projectRepository;
            private readonly IRepository<ReferenceOthers, Guid> _referenceOthersRepository;

            private readonly IRepository<ContactCompany, Guid> _contactCompanyRepository;


        public ImportAppService(IRepository<Reference, Guid> referenceModelRepository,
                                        IRepository<ReferenceType, Guid> referenceTypeModelRepository,
                                        IRepository<TenderBasic, Guid> tenderBasicRepository,
                                        IRepository<Project, Guid> projectRepository,
                                        IRepository<ReferenceOthers, Guid> referenceOthersRepository,

                                        IRepository<ContactCompany, Guid> contactCompanyRepository
            )
            {
                this._referenceModelRepository = referenceModelRepository;
                this._referenceTypeModelRepository = referenceTypeModelRepository;
                this._tenderBasicRepository = tenderBasicRepository;
                this._projectRepository = projectRepository;
                this._referenceOthersRepository = referenceOthersRepository;

                this._contactCompanyRepository = contactCompanyRepository;
        }

        #region Import Reference
        [HttpPost]
        public async Task<string> ImportExcelReference([FromForm] IFormFile excel)
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
                var list = new List<Reference>();

                using (var stream = new MemoryStream())
                {
                    await excel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        var excelColoumn = new Dictionary<string, int>() {
                            {"code",0},
                            {"title",0},
                            {"description",0},
                            {"slno",0},
                            {"symbol",0},
                            {"year",0},
                            {"referencetypeid",0},
                            {"foreignkeyid",0},
                        };

                        //for(var i in worksheet.Column)
                        var coloumnCount = worksheet.Dimension.End.Column;
                        for (int i = 1; i <= coloumnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value.ToString().Equals("code", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["code"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("title", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["title"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("description", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["description"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("slno", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["slno"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("symbol", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["symbol"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("year", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["year"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("referencetypeid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["referencetypeid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("foreignkeyid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["foreignkeyid"] = i;
                            }

                        }


                        var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("code", StringComparison.OrdinalIgnoreCase);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var Code = worksheet.Cells[row, excelColoumn["code"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["code"]].Value.ToString().Trim();

                            var Title = worksheet.Cells[row, excelColoumn["title"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["title"]].Value.ToString().Trim();

                            var Description = worksheet.Cells[row, excelColoumn["description"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["description"]].Value.ToString().Trim();

                            var SlNo = worksheet.Cells[row, excelColoumn["slno"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["slno"]].Value.ToString().Trim();

                            var Symbol = worksheet.Cells[row, excelColoumn["symbol"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["symbol"]].Value.ToString().Trim();

                            var Year = worksheet.Cells[row, excelColoumn["year"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["year"]].Value.ToString().Trim();

                            var ReferenceTypeId = worksheet.Cells[row, excelColoumn["referencetypeid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["referencetypeid"]].Value.ToString().Trim();

                            var ForeignKeyId = worksheet.Cells[row, excelColoumn["foreignkeyid"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["foreignkeyid"]].Value.ToString().Trim();

                            //var ForId = ForeignKeyGetDetails(Code, ReferenceTypeId).Result;
                            //var refTypeId = ReferenceTypeGetDetails(ReferenceTypeId).Result;
                            list.Add(new Reference
                            {
                                Code = Code,
                                Title = Title,
                                Description = Description,
                                SlNo = SlNo==""?0:Int32.Parse(SlNo),
                                Symbol = Symbol,
                                Year = Year==""?0:Int32.Parse(Year),
                                ReferenceTypeId = Guid.Parse(ReferenceTypeId),
                                ForeignKeyId = Guid.Parse(ForeignKeyId)
                            }); 
                        }
                    }
                }
                if (list.Count > 0)
                {
                    await _referenceModelRepository.InsertManyAsync(list);
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
        public async Task<Guid> ReferenceTypeGetDetails(string referenceTypeSymbol)
        {
            var data = _referenceTypeModelRepository.Where(x => x.Symbol.Equals(referenceTypeSymbol)).FirstOrDefault();
            if (data is null)
            {
                return Guid.Parse("49871529-ECB3-4E2A-B4E3-536C9714BA3E");
            }
            try
            {
                return Guid.Parse(data.Id.ToString());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<Guid> ForeignKeyGetDetails(string Code, string ReferenceTypeId)
        {
            var data = _referenceTypeModelRepository.Where(x => x.Symbol.Equals(ReferenceTypeId)).FirstOrDefault();

            try
            {
                if (data is null)
                {
                    return  Guid.Parse("00000000-0000-0000-0000-000000000000");
                }
                else
                {
                    if (ReferenceTypeId == "P")
                    {
                        var data1 = _projectRepository.Where(x => x.ReferenceCode.Equals(Code)).FirstOrDefault();
                        if (data1 is null)
                        {
                            return Guid.Parse("00000000-0000-0000-0000-000000000000");
                        }
                        else
                        {
                            return Guid.Parse(data1.Id.ToString());
                        }
                    }
                    else if (ReferenceTypeId == "T")
                    {
                        var data1 = _tenderBasicRepository.Where(x => x.ReferenceCode.Equals(Code)).FirstOrDefault();
                        if (data1 is null)
                        {
                            return Guid.Parse("00000000-0000-0000-0000-000000000000");
                        }
                        else
                        {
                            return Guid.Parse(data1.Id.ToString());
                        }
                    }
                    else if (ReferenceTypeId == "O")
                    {
                        var data1 = _referenceOthersRepository.Where(x => x.ReferenceCode.Equals(Code)).FirstOrDefault();
                        if (data1 is null)
                        {
                            return Guid.Parse("00000000-0000-0000-0000-000000000000");
                        }
                        else
                        {
                            return Guid.Parse(data1.Id.ToString());
                        }
                    }
                    else
                    {
                        return Guid.Parse("00000000-0000-0000-0000-000000000000");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            

        }
        #endregion Import


        #region Import Project
        [HttpPost]
        public async Task<string> ImportExcelProject([FromForm] IFormFile excel)
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
                var list = new List<Project>();

                using (var stream = new MemoryStream())
                {
                    await excel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        var excelColoumn = new Dictionary<string, int>() {
                            {"oldid",0},
                            {"referencecode",0},
                            {"title",0},
                            { "description",0},
                            { "overview",0},
                        };

                        //for(var i in worksheet.Column)
                        var coloumnCount = worksheet.Dimension.End.Column;
                        for (int i = 1; i <= coloumnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("referencecode", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["referencecode"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("title", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["title"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("description", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["description"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("overview", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["overview"] = i;
                            }
                        }


                        var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("oldid", StringComparison.OrdinalIgnoreCase);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var OldId = worksheet.Cells[row, excelColoumn["oldid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldid"]].Value.ToString().Trim();

                            var ReferenceCode = worksheet.Cells[row, excelColoumn["referencecode"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["referencecode"]].Value.ToString().Trim();

                            var Title = worksheet.Cells[row, excelColoumn["title"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["title"]].Value.ToString().Trim();

                            var Description = worksheet.Cells[row, excelColoumn["description"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["description"]].Value.ToString().Trim();

                            var Overview = worksheet.Cells[row, excelColoumn["overview"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["overview"]].Value.ToString().Trim();

                            list.Add(new Project
                            {
                                OldId = OldId,
                                ReferenceCode = ReferenceCode,
                                Title = Title,
                                Description = Description,
                                ProjectCost=0,
                                Overview= Overview,
                                TechnicalFeatures = "",
                                ClientId = Guid.Parse("CC61C0BA-2207-0575-3E97-39FD501F6872"),
                                CurrencyId = Guid.Parse("567BBF8A-BD7F-4144-AAFB-528BA9F7F248"),
                                ModalityId = Guid.Parse("8F9374DB-BEE4-417C-A1FF-1FC518CEEEEC"),
                                ProjectStatusId = Guid.Parse("72ADB30E-ED01-41AA-A3EB-BA5A71A1A0C1"),
                            }); ;
                        }
                    }
                }
                if (list.Count > 0)
                {
                    await _projectRepository.InsertManyAsync(list);
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
        #endregion


        #region Import Tender
        [HttpPost]
        public async Task<string> ImportExcelTender([FromForm] IFormFile excel)
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
                var list = new List<TenderBasic>();

                using (var stream = new MemoryStream())
                {
                    await excel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        var excelColoumn = new Dictionary<string, int>() {
                            {"oldid",0},
                            {"referencecode",0},
                            {"title",0},
                            {"clientid",0},
                            { "oldbudget",0},
                            { "clientreference",0},
                            { "detaildescription",0},
                            { "primaryinchargeid",0},
                            { "budget",0},
                        };

                        //for(var i in worksheet.Column)
                        var coloumnCount = worksheet.Dimension.End.Column;
                        for (int i = 1; i <= coloumnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("referencecode", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["referencecode"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("title", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["title"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("clientid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["clientid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("budget", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["budget"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("clientreference", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["clientreference"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldbudget", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldbudget"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("clientreference", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["clientreference"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("detaildescription", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["detaildescription"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("primaryinchargeid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["primaryinchargeid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("budget", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["budget"] = i;
                            }
                        }


                        var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("oldid", StringComparison.OrdinalIgnoreCase);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var OldId = worksheet.Cells[row, excelColoumn["oldid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldid"]].Value.ToString().Trim();

                            var ReferenceCode = worksheet.Cells[row, excelColoumn["referencecode"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["referencecode"]].Value.ToString().Trim();

                            var Title = worksheet.Cells[row, excelColoumn["title"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["title"]].Value.ToString().Trim();

                            var ClientId = worksheet.Cells[row, excelColoumn["clientid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["clientid"]].Value.ToString().Trim();

                            var OldBudget = worksheet.Cells[row, excelColoumn["oldbudget"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldbudget"]].Value.ToString().Trim();

                            var ClientReference = worksheet.Cells[row, excelColoumn["clientreference"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["clientreference"]].Value.ToString().Trim();

                            var DetailDescription = worksheet.Cells[row, excelColoumn["detaildescription"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["detaildescription"]].Value.ToString().Trim();

                            var PrimaryInchargeid = worksheet.Cells[row, excelColoumn["primaryinchargeid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["primaryinchargeid"]].Value.ToString().Trim();

                            var Budget = worksheet.Cells[row, excelColoumn["budget"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["budget"]].Value.ToString().Trim();



                            var CId = ClientIdGetDetails(ClientId).Result;
                            //var PIId = ClientIdGetDetails(PrimaryInchargeid).Result;
                            list.Add(new TenderBasic
                            {
                                OldId = OldId,
                                ReferenceCode = ReferenceCode,
                                TenderStageId = Guid.Parse("865E3754-71E2-4099-B14D-0AD38297F18D"),
                                TenderTypeId = Guid.Parse("85F12178-A7B7-4888-8A7E-6C99686AC8AE"),
                                SectorId = Guid.Parse("966DC817-F31A-4AAA-9F84-08924C170593"),
                                PrimaryInchargeId = Guid.Parse("73FC9457-6B63-6A4A-64DE-39F960722731"),
                                Title = Title,
                                ProjectTitle = null,
                                ClientId = CId,
                                Budget = decimal.Parse(Budget),
                                BudgetCurrencyId = Guid.Parse("567BBF8A-BD7F-4144-AAFB-528BA9F7F248"),
                                Department = "",
                                Note = OldBudget,
                                OfficialInvitation = true,
                                DocumentCostCurrencyId = Guid.Parse("567BBF8A-BD7F-4144-AAFB-528BA9F7F248"),
                                DocumentCost = 0,
                                BankGuranteeValueCurrencyId = Guid.Parse("567BBF8A-BD7F-4144-AAFB-528BA9F7F248"),
                                BankGuranteeValue = 0,
                                PublicationDate = null,
                                SubmissionDate = null,
                                OpeningDate = null,
                                BidValidity = null,
                                BankGuranteeValidity = null,
                                ClientReference = ClientReference,
                                DetailDescription = DetailDescription,

                            }); ;
                        }
                    }
                }
                if (list.Count > 0)
                {
                    await _tenderBasicRepository.InsertManyAsync(list);
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
        public async Task<Guid> ClientIdGetDetails(string ClientId)
        {
            try
            {
                var data = _contactCompanyRepository.Where(x => x.OldId.Equals(int.Parse(ClientId))).FirstOrDefault();

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

        #endregion

        #region Import Other
        [HttpPost]
        public async Task<string> ImportExcelOther([FromForm] IFormFile excel)
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
                var list = new List<ReferenceOthers>();

                using (var stream = new MemoryStream())
                {
                    await excel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        var excelColoumn = new Dictionary<string, int>() {
                            {"oldid",0},
                            {"referencecode",0},
                            {"title",0},
                            {"description",0},
                        };

                        //for(var i in worksheet.Column)
                        var coloumnCount = worksheet.Dimension.End.Column;
                        for (int i = 1; i <= coloumnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value.ToString().Equals("oldid", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["oldid"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("referencecode", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["referencecode"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("title", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["title"] = i;
                            }
                            if (worksheet.Cells[1, i].Value.ToString().Equals("description", StringComparison.OrdinalIgnoreCase))
                            {
                                excelColoumn["description"] = i;
                            }
                        }


                        var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("oldid", StringComparison.OrdinalIgnoreCase);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var OldId = worksheet.Cells[row, excelColoumn["oldid"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["oldid"]].Value.ToString().Trim();

                            var ReferenceCode = worksheet.Cells[row, excelColoumn["referencecode"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["referencecode"]].Value.ToString().Trim();

                            var Title = worksheet.Cells[row, excelColoumn["title"]].Value is null
                            ? string.Empty
                            : worksheet.Cells[row, excelColoumn["title"]].Value.ToString().Trim();

                            var Description = worksheet.Cells[row, excelColoumn["description"]].Value is null
                                ? string.Empty
                                : worksheet.Cells[row, excelColoumn["description"]].Value.ToString().Trim();

                            list.Add(new ReferenceOthers
                            {
                                OldId = OldId,
                                ReferenceCode = ReferenceCode,
                                Title = Title,
                                Description = Description,

                            }); ;
                        }
                    }
                }
                if (list.Count > 0)
                {
                    await _referenceOthersRepository.InsertManyAsync(list);
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

        #endregion




        //#region Import test
        //[HttpPost]
        //public async Task<string> ImportExcel([FromForm] IFormFile excel)
        //{
        //    try
        //    {
        //        if (excel == null | excel.Length <= 0)
        //        {
        //            throw new UserFriendlyException("File not found");
        //        }
        //        if (!Path.GetExtension(excel.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        //        {
        //            throw new UserFriendlyException("File fotmat not correct");
        //        }
        //        var list = new List<Reference>();

        //        using (var stream = new MemoryStream())
        //        {
        //            await excel.CopyToAsync(stream);
        //            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //            using (var package = new ExcelPackage(stream))
        //            {

        //                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

        //                var excelColoumn = new Dictionary<string, int>() {
        //                {"code",0},
        //                { "title",0},
        //                { "description",0},
        //            };

        //                //for(var i in worksheet.Column)
        //                var coloumnCount = worksheet.Dimension.End.Column;
        //                for (int i = 1; i <= coloumnCount; i++)
        //                {
        //                    if (worksheet.Cells[1, i].Value.ToString().Equals("code", StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        excelColoumn["code"] = i;
        //                    }
        //                    if (worksheet.Cells[1, i].Value.ToString().Equals("title", StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        excelColoumn["title"] = i;
        //                    }
        //                    if (worksheet.Cells[1, i].Value.ToString().Equals("description", StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        excelColoumn["description"] = i;
        //                    }
        //                }


        //                var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("code", StringComparison.OrdinalIgnoreCase);
        //                var rowCount = worksheet.Dimension.Rows;
        //                for (int row = 2; row <= rowCount; row++)
        //                {

        //                    var Code = worksheet.Cells[row, excelColoumn["code"]].Value is null
        //                        ? string.Empty
        //                        : worksheet.Cells[row, excelColoumn["code"]].Value.ToString().Trim();

        //                    var Title = worksheet.Cells[row, excelColoumn["title"]].Value is null
        //                        ? string.Empty
        //                        : worksheet.Cells[row, excelColoumn["title"]].Value.ToString().Trim();

        //                    var Description = worksheet.Cells[row, excelColoumn["description"]].Value is null
        //                        ? string.Empty
        //                        : worksheet.Cells[row, excelColoumn["description"]].Value.ToString().Trim();


        //                    list.Add(new Reference
        //                    {
        //                        Code = Code,
        //                        Title = Title,
        //                        Description = Description
        //                    });
        //                }
        //            }

        //        }
        //        if (list.Count > 0)
        //        {
        //            await _referenceModelRepository.InsertManyAsync(list);
        //            await CurrentUnitOfWork.SaveChangesAsync();
        //            return "Sucessfully Uploaded in database";
        //        }

        //        return "Excel file couldnot be uploaded";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(ex.Message);
        //    }
        //}
        //#endregion Import
    }
}
