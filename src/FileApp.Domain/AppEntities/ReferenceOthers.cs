using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities
{
    public class ReferenceOthers : AuditedAggregateRoot<Guid>
    {
        public string OldId { get; set; }
        public string ReferenceCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
