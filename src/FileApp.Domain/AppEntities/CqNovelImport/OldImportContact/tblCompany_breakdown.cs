using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities.CqNovelImport.OldImportContact
{
    public class tblCompany_breakdown : AuditedAggregateRoot<Guid>
    {
        public float F1 { get; set; }
        public int OldId { get; set; }
        public string CompanyAbbrv { get; set; }
        public string CompanyName { get; set; }
        public string Sectionof { get; set; }
        public string Sowupdatedby { get; set; }
        public string ISOTXT { get; set; }
        public string LASTUPDATE { get; set; }
        public float INACTIVE { get; set; }
        public float BranchofCompany { get; set; }
        public float SourceOfData { get; set; }
        public string accessibleTo { get; set; }
        public string SowLevel1 { get; set; }
        public string SowLevel2 { get; set; }
        public string SowAssigned { get; set; }
        public string PERMANENTTEMPORARY { get; set; }
        public string Notes { get; set; }
        public string MailAddress { get; set; }
        public int CountryID { get; set; }
        public int CityID { get; set; }
        public string comp_natureid { get; set; }
        public string comp_typesid { get; set; }
        public string comp_ceo { get; set; }
        public float comp_archived { get; set; }
        public string comp_postboxnum { get; set; }
        public string comp_postboxnum2 { get; set; }
        public float comp_npagentid { get; set; }
        public string comp_npexperience { get; set; }
        public string comp_closingremarks { get; set; }
        public float comp_regdby { get; set; }
        public float comp_updatedby { get; set; }
        public string comp_logo { get; set; }
        public int defaultthrough { get; set; }
        public int detailEntryRequired { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DeletedDate { get; set; }
        public string company_updatedby { get; set; }
        public string company_regdby { get; set; }
        public string dTag { get; set; }
    }
}
