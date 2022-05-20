using FileApp.AppEntities.CqNovelImport.ImportContact;
using FileApp.AppEntities.CqNovelImport.ImportContact.SystemTable;
using FileApp.AppEntities.CqNovelImport.OldImportContact;
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

namespace FileApp.CqNovelImport.OldImportContact
{
    public class OldImportContactAppService : ApplicationService, IImportOldContactAppService
    {
        private readonly IRepository<tblCompany_breakdown, Guid> _oldCompanyRepository;


        public OldImportContactAppService(IRepository<tblCompany_breakdown, Guid> oldCompanyRepository)
        {
            this._oldCompanyRepository = oldCompanyRepository;

        }
        #region 1 Import ContactCompany
        [HttpPost]
        public async Task<string> ImportExcelOldContactCompany([FromForm] IFormFile excel)
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
                var list = new List<tblCompany_breakdown>();

                using (var stream = new MemoryStream())
                {
                    await excel.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        var excelColoumn = new Dictionary<string, int>() {
                            {"f1",0},
                            {"id",0},
                            {"companyabbrv",0},
                            {"companyname",0},
                            {"sectionof",0},
                            {"sowupdatedby",0},
                            {"isotxt",0},
                            {"lastupdate",0},
                            {"inactive",0},
                            {"branchofcompany",0},
                            {"sourceofdata",0},
                            {"accessibleto",0},
                            {"sowlevel1",0},
                            {"sowlevel2",0},
                            {"sowassigned",0},
                            {"permanenttemporary",0},
                            {"notes",0},
                            {"mailaddress",0},
                            {"countryid",0},
                            {"cityid",0},
                            {"comp_natureid",0},
                            {"comp_typesid",0},
                            {"comp_ceo",0},
                            {"comp_archived",0},
                            {"comp_postboxnum",0},
                            {"comp_postboxnum2",0},
                            {"comp_npagentid",0},
                            {"comp_npexperience",0},
                            {"comp_closingremarks",0},
                            {"comp_regdby",0},
                            {"comp_updatedby",0},
                            {"comp_logo",0},
                            {"defaultthrough",0},
                            {"detailentryrequired",0},
                            {"entrydate",0},
                            {"deleteddate",0},
                            {"company_updatedby",0},
                            {"company_regdby",0},
                            {"dtag",0}
                        };
                        //for(var i in worksheet.Column)
                        var coloumnCount = worksheet.Dimension.End.Column;
                        for (int i = 1; i <= coloumnCount; i++)
                        {
                            if (worksheet.Cells[1, i].Value.ToString().Equals("f1", StringComparison.OrdinalIgnoreCase)){excelColoumn["f1"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("id", StringComparison.OrdinalIgnoreCase)){excelColoumn["id"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("companyabbrv", StringComparison.OrdinalIgnoreCase)){excelColoumn["companyabbrv"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("companyname", StringComparison.OrdinalIgnoreCase)){excelColoumn["companyname"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("sectionof", StringComparison.OrdinalIgnoreCase)){excelColoumn["sectionof"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("sowupdatedby", StringComparison.OrdinalIgnoreCase)){excelColoumn["sowupdatedby"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("isotxt", StringComparison.OrdinalIgnoreCase)){excelColoumn["isotxt"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("lastupdate", StringComparison.OrdinalIgnoreCase)){excelColoumn["lastupdate"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("inactive", StringComparison.OrdinalIgnoreCase)){excelColoumn["inactive"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("branchofcompany", StringComparison.OrdinalIgnoreCase)){excelColoumn["branchofcompany"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("sourceofdata", StringComparison.OrdinalIgnoreCase)){excelColoumn["sourceofdata"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("accessibleto", StringComparison.OrdinalIgnoreCase)){excelColoumn["accessibleto"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("sowlevel1", StringComparison.OrdinalIgnoreCase)){excelColoumn["sowlevel1"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("sowlevel2", StringComparison.OrdinalIgnoreCase)){excelColoumn["sowlevel2"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("sowassigned", StringComparison.OrdinalIgnoreCase)){excelColoumn["sowassigned"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("permanenttemporary", StringComparison.OrdinalIgnoreCase)){excelColoumn["permanenttemporary"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("notes", StringComparison.OrdinalIgnoreCase)){excelColoumn["notes"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("mailaddress", StringComparison.OrdinalIgnoreCase)){excelColoumn["mailaddress"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("countryid", StringComparison.OrdinalIgnoreCase)){excelColoumn["countryid"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("cityid", StringComparison.OrdinalIgnoreCase)){excelColoumn["cityid"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_natureid", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_natureid"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_typesid", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_typesid"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_ceo", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_ceo"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_archived", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_archived"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_postboxnum", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_postboxnum"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_postboxnum2", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_postboxnum2"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_npagentid", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_npagentid"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_npexperience", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_npexperience"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_closingremarks", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_closingremarks"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_regdby", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_regdby"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_updatedby", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_updatedby"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("comp_logo", StringComparison.OrdinalIgnoreCase)){excelColoumn["comp_logo"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("defaultthrough", StringComparison.OrdinalIgnoreCase)){excelColoumn["defaultthrough"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("detailentryrequired", StringComparison.OrdinalIgnoreCase)){excelColoumn["detailentryrequired"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("entrydate", StringComparison.OrdinalIgnoreCase)){excelColoumn["entrydate"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("deleteddate", StringComparison.OrdinalIgnoreCase)){excelColoumn["deleteddate"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("company_updatedby", StringComparison.OrdinalIgnoreCase)){excelColoumn["company_updatedby"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("company_regdby", StringComparison.OrdinalIgnoreCase)){excelColoumn["company_regdby"] = i;}
                            if (worksheet.Cells[1, i].Value.ToString().Equals("dtag", StringComparison.OrdinalIgnoreCase)){excelColoumn["dtag"] = i;}
                        }


                        var cityColoumn = worksheet.Cells[1, 1].Value.ToString().Trim().Equals("companyname", StringComparison.OrdinalIgnoreCase);
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {

                            var f1 = worksheet.Cells[row, excelColoumn["f1"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["f1"]].Value.ToString().Trim();
                            var id = worksheet.Cells[row, excelColoumn["id"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["id"]].Value.ToString().Trim();
                            var companyabbrv = worksheet.Cells[row, excelColoumn["companyabbrv"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["companyabbrv"]].Value.ToString().Trim();
                            var companyname = worksheet.Cells[row, excelColoumn["companyname"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["companyname"]].Value.ToString().Trim();
                            var sectionof = worksheet.Cells[row, excelColoumn["sectionof"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["sectionof"]].Value.ToString().Trim();
                            var sowupdatedby = worksheet.Cells[row, excelColoumn["sowupdatedby"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["sowupdatedby"]].Value.ToString().Trim();
                            var isotxt = worksheet.Cells[row, excelColoumn["isotxt"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["isotxt"]].Value.ToString().Trim();
                            var lastupdate = worksheet.Cells[row, excelColoumn["lastupdate"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["lastupdate"]].Value.ToString().Trim();
                            var inactive = worksheet.Cells[row, excelColoumn["inactive"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["inactive"]].Value.ToString().Trim();
                            var branchofcompany = worksheet.Cells[row, excelColoumn["branchofcompany"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["branchofcompany"]].Value.ToString().Trim();
                            var sourceofdata = worksheet.Cells[row, excelColoumn["sourceofdata"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["sourceofdata"]].Value.ToString().Trim();
                            var accessibleto = worksheet.Cells[row, excelColoumn["accessibleto"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["accessibleto"]].Value.ToString().Trim();
                            var sowlevel1 = worksheet.Cells[row, excelColoumn["sowlevel1"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["sowlevel1"]].Value.ToString().Trim();
                            var sowlevel2 = worksheet.Cells[row, excelColoumn["sowlevel2"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["sowlevel2"]].Value.ToString().Trim();
                            var sowassigned = worksheet.Cells[row, excelColoumn["sowassigned"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["sowassigned"]].Value.ToString().Trim();
                            var permanenttemporary = worksheet.Cells[row, excelColoumn["permanenttemporary"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["permanenttemporary"]].Value.ToString().Trim();
                            var notes = worksheet.Cells[row, excelColoumn["notes"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["notes"]].Value.ToString().Trim();
                            var mailaddress = worksheet.Cells[row, excelColoumn["mailaddress"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["mailaddress"]].Value.ToString().Trim();
                            var countryid = worksheet.Cells[row, excelColoumn["countryid"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["countryid"]].Value.ToString().Trim();
                            var cityid = worksheet.Cells[row, excelColoumn["cityid"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["cityid"]].Value.ToString().Trim();
                            var comp_natureid = worksheet.Cells[row, excelColoumn["comp_natureid"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_natureid"]].Value.ToString().Trim();
                            var comp_typesid = worksheet.Cells[row, excelColoumn["comp_typesid"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_typesid"]].Value.ToString().Trim();
                            var comp_ceo = worksheet.Cells[row, excelColoumn["comp_ceo"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_ceo"]].Value.ToString().Trim();
                            var comp_archived = worksheet.Cells[row, excelColoumn["comp_archived"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_archived"]].Value.ToString().Trim();
                            var comp_postboxnum = worksheet.Cells[row, excelColoumn["comp_postboxnum"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_postboxnum"]].Value.ToString().Trim();
                            var comp_postboxnum2 = worksheet.Cells[row, excelColoumn["comp_postboxnum2"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_postboxnum2"]].Value.ToString().Trim();
                            var comp_npagentid = worksheet.Cells[row, excelColoumn["comp_npagentid"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_npagentid"]].Value.ToString().Trim();
                            var comp_npexperience = worksheet.Cells[row, excelColoumn["comp_npexperience"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_npexperience"]].Value.ToString().Trim();
                            var comp_closingremarks = worksheet.Cells[row, excelColoumn["comp_closingremarks"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_closingremarks"]].Value.ToString().Trim();
                            var comp_regdby = worksheet.Cells[row, excelColoumn["comp_regdby"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_regdby"]].Value.ToString().Trim();
                            var comp_updatedby = worksheet.Cells[row, excelColoumn["comp_updatedby"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_updatedby"]].Value.ToString().Trim();
                            var comp_logo = worksheet.Cells[row, excelColoumn["comp_logo"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["comp_logo"]].Value.ToString().Trim();
                            var defaultthrough = worksheet.Cells[row, excelColoumn["defaultthrough"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["defaultthrough"]].Value.ToString().Trim();
                            var detailentryrequired = worksheet.Cells[row, excelColoumn["detailentryrequired"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["detailentryrequired"]].Value.ToString().Trim();
                            var entrydate = worksheet.Cells[row, excelColoumn["entrydate"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["entrydate"]].Text.Trim();
                            var deleteddate = worksheet.Cells[row, excelColoumn["deleteddate"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["deleteddate"]].Text.Trim();
                            var company_updatedby = worksheet.Cells[row, excelColoumn["company_updatedby"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["company_updatedby"]].Value.ToString().Trim();
                            var company_regdby = worksheet.Cells[row, excelColoumn["company_regdby"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["company_regdby"]].Value.ToString().Trim();
                            var dtag = worksheet.Cells[row, excelColoumn["dtag"]].Value is null? string.Empty: worksheet.Cells[row, excelColoumn["dtag"]].Value.ToString().Trim();

                            list.Add(new tblCompany_breakdown
                            {

                                F1=int.Parse(f1),
                                OldId=int.Parse(id),
                                CompanyAbbrv=companyabbrv,
                                CompanyName=companyname,
                                Sectionof=sectionof,
                                Sowupdatedby=sowupdatedby,
                                ISOTXT=isotxt,
                                LASTUPDATE=lastupdate,
                                INACTIVE=int.Parse(inactive),
                                BranchofCompany= int.Parse(branchofcompany),
                                SourceOfData= int.Parse(sourceofdata),
                                accessibleTo=accessibleto,
                                SowLevel1=sowlevel1,
                                SowLevel2=sowlevel2,
                                SowAssigned=sowassigned,
                                PERMANENTTEMPORARY=permanenttemporary,
                                Notes=notes,
                                MailAddress=mailaddress,
                                CountryID= int.Parse(countryid),
                                CityID= int.Parse(cityid),
                                comp_natureid=comp_natureid,
                                comp_typesid=comp_typesid,
                                comp_ceo=comp_ceo,
                                comp_archived= int.Parse(comp_archived),
                                comp_postboxnum=comp_postboxnum,
                                comp_postboxnum2=comp_postboxnum2,
                                comp_npagentid= int.Parse(comp_npagentid),
                                comp_npexperience=comp_npexperience,
                                comp_closingremarks=comp_closingremarks,
                                comp_regdby= int.Parse(comp_regdby),
                                comp_updatedby= int.Parse(comp_updatedby),
                                comp_logo=comp_logo,
                                defaultthrough= int.Parse(defaultthrough),
                                detailEntryRequired= int.Parse(detailentryrequired),
                                EntryDate=DateTime.Parse(entrydate),
                                DeletedDate=DateTime.Parse(deleteddate),
                                company_updatedby=company_updatedby,
                                company_regdby=company_regdby,
                                dTag = dtag
                            });
                        }
                    }
                }
                if (list.Count > 0)
                {
                    await _oldCompanyRepository.InsertManyAsync(list);
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
