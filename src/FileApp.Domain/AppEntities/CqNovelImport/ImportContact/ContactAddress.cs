
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities.CqNovelImport.ImportContact
{
    public class ContactAddress : AuditedAggregateRoot<Guid>
    {
        public string OldId { get; set; }
        public Guid? ContactPersonId { get; set; }
        public Guid? ContactCompanyId { get; set; }

        public Guid ContactLabelId { get; set; }
        public Guid CountryId { get; set; }

        public Guid CityId { get; set; }
        public string StreetName { get; set; }
        public string PoBox { get; set; }
        public string StateProvince { get; set; }
        public string PostalCodeZip { get; set; }

        public bool IsPrimaryAddress { get; set; }
        public bool IsActive { get; set; }
    }
}
