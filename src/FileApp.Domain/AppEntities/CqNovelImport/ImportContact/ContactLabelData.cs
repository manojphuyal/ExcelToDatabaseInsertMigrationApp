using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities.CqNovelImport.ImportContact
{
    public class ContactLabelData : AuditedAggregateRoot<Guid>
    {
        public string OldId { get; set; }
        public string LabelData { get; set; }

        public Guid ContactLabelId { get; set; }

        public Guid? ContactPersonId { get; set; }

        public Guid? ContactCompanyId { get; set; }

        public bool IsActive { get; set; }
    }
}
