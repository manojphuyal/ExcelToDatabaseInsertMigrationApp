using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities
{
    public class Project : AuditedAggregateRoot<Guid>
    {
        public string OldId { get; set; }
        public string ReferenceCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal ProjectCost { get; set; }
        public string Overview { get; set; }
        public string TechnicalFeatures { get; set; }
        public Guid ClientId { get; set; }  //Client Table ko Company Id
        public Guid CurrencyId { get; set; }
        public Guid ModalityId { get; set; }
        public Guid ProjectStatusId { get; set; }
    }
}
