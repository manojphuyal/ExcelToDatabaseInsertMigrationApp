using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace FileApp.AppEntities.CqNovelImport.ImportContact.SystemTable
{
    public class Gender : Entity<Guid>
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }

        public int DisplayOrder { get; set; }
    }
}
