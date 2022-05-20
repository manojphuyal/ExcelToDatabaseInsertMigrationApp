using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace FileApp.AppEntities
{
    public class TenderBasic : AuditedAggregateRoot<Guid>
    {
        public string OldId { get; set; }
        public string ReferenceCode { get; set; }
        public Guid TenderStageId { get; set; }
        public Guid TenderTypeId { get; set; }
        public Guid SectorId { get; set; }
        public Guid PrimaryInchargeId { get; set; }
        public string Title { get; set; }
        public string ProjectTitle { get; set; } //Project If Any
        public Guid ClientId { get; set; }
        public decimal Budget { get; set; }
        public Guid BudgetCurrencyId { get; set; }
        public string Department { get; set; }
        public string Note { get; set; }
        public bool OfficialInvitation { get; set; }
        public Guid? DocumentCostCurrencyId { get; set; }
        public decimal? DocumentCost { get; set; }
        public Guid? BankGuranteeValueCurrencyId { get; set; }
        public decimal? BankGuranteeValue { get; set; }
        public DateTime? PublicationDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public DateTime? OpeningDate { get; set; }
        public DateTime? BidValidity { get; set; }
        public DateTime? BankGuranteeValidity { get; set; }
        public string ClientReference { get; set; }
        public string? DetailDescription { get; set; }

    }
}
