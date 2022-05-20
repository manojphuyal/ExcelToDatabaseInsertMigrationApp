using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities
{
        public class NameModel : AuditedAggregateRoot<Guid>
        {
            public string Name { get; set; }
            public string Class { get; set; }
            public string Roll { get; set; }
            public string Number { get; set; }
        }
}
