using System;
using System.Collections.Generic;
using System.Text;

namespace FileApp.CqNovelImport.ImportContact
{
    public class ContactCompanyAddUpdateDto
    {
        public string CompanyName { get; set; }
        public string CompanyAbbrevation { get; set; }

        public Guid? ParentCompanyId { get; set; }
        public Guid? ContactSourceId { get; set; }
        public string DefaultThrough { get; set; }
        public string Notes { get; set; }
        public string ScopeOfWork { get; set; }
        public string MailAddress { get; set; }
        public string CompanyCEO { get; set; }
        public bool IsQuickEntry { get; set; }
        public string TagNames { get; set; }

        public bool IsActive { get; set; }
    }
}
