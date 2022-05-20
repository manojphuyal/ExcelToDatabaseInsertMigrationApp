using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities.CqNovelImport.ImportContact
{
    public class ContactSource : AuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
