
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities.CqNovelImport.ImportContact
{
    public class ContactPerson : AuditedAggregateRoot<Guid>
    {
        public string OldId { get; set; }
        public string OldCityId { get; set; }
        public string OldCountryId { get; set; }
        public string Suffix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string PersonalAbbrevation { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public string SpouseName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string Hobbies { get; set; }
        public string Notes { get; set; }

        public Guid? GenderId { get; set; }

        public Guid? MaritalStatusId { get; set; }

        public Guid? CompanyContactId { get; set; }

        public Guid? ConatactSourceId { get; set; }

        public string DefaultThrough { get; set; }
        public bool IsQuickEntry { get; set; }
        public string TagNames { get; set; }

        public bool IsActive { get; set; }

    }
}
