using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities
{
    public class ReferenceType : AuditedAggregateRoot<Guid>
    {
        public string ReferenceTypeName { get; set; }

        public string Symbol { get; set; }
    }
}
