using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities.CqNovelImport.ImportContact.SystemTable
{
    public class City : AuditedAggregateRoot<Guid>
    {
        public string OldId { get; set; }
        public string Name { get; set; }
        public Guid CountryId { get; set; }
        public bool IsActive { get; set; }
    }
}
