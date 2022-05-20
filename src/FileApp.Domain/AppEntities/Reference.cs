using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities
{
    public class Reference : AuditedAggregateRoot<Guid>
    {
        //public string OldId { get; set; }

        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SlNo { get; set; }
        public string Symbol { get; set; }
        public int Year { get; set; }
        public Guid ReferenceTypeId { get; set; }
        public Guid ForeignKeyId { get; set; }//Pk value of Tables
        public string KeyWords { get; set; }


    }
}
