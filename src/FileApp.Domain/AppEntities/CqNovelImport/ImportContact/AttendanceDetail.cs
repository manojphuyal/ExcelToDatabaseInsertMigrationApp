using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities.CqNovelImport.ImportContact
{
    public class AttendanceDetail : AuditedAggregateRoot<Guid>
    {
        public Guid AppUserId { get; set; }
        public DateTime AttendanceIn { get; set; }
        public DateTime? AttendanceOutAt { get; set; }
        public bool IsHalfDay { get; set; }
        public bool IsActive { get; set; }

        public bool IsInGracePeriod { get; set; }
        public Guid? DailyloginDetailId { get; set; }
        public bool IsFingerprint { get; set; }
        public Guid AttendanceTypeId { get; set; }
    }
}
