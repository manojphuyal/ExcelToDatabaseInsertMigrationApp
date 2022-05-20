using FileApp.AppEntities;
using FileApp.AppEntities.CqNovelImport.ImportContact;
using FileApp.AppEntities.CqNovelImport.ImportContact.SystemTable;
using FileApp.AppEntities.CqNovelImport.OldImportContact;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace FileApp.EntityFrameworkCore
{
    public static class FileAppDbContextModelCreatingExtensions
    {
        public static void ConfigureFileApp(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(FileAppConsts.DbTablePrefix + "YourEntities", FileAppConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
            builder.Entity<NameModel>(b =>
            {
                b.ToTable("NameModel");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();

                b.Property(p => p.Name).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.Roll).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.Class).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.Number).IsRequired(false).HasMaxLength(400);
            });

            builder.Entity<Reference>(b =>
            {
                b.ToTable("References");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();
                b.Property(p => p.ReferenceTypeId).IsRequired(true);
                b.Property(p => p.ForeignKeyId).IsRequired(true);
                b.Property(p => p.SlNo).IsRequired(true);
                b.Property(p => p.Code).IsRequired(true);
                b.Property(p => p.Year).IsRequired(true);
                b.Property(p => p.KeyWords).IsRequired(false);
                b.Property(p => p.Description).IsRequired(false);
                b.Property(p => p.ReferenceTypeId).IsRequired(true);

            });


            builder.Entity<ReferenceType>(b =>
            {
                b.ToTable("ReferenceTypes");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();
                b.Property(p => p.ReferenceTypeName).IsRequired(true);
            });

            builder.Entity<Project>(b =>
            {
                b.ToTable("Projects");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();
                b.Property(p => p.Title).IsRequired(true).HasMaxLength(400);
                b.Property(p => p.Description).IsRequired(true);
                b.Property(p => p.ProjectCost).IsRequired(true);
                b.Property(p => p.Overview).IsRequired(true);
                b.Property(p => p.TechnicalFeatures).IsRequired(true);
                b.Property(p => p.CurrencyId).IsRequired(true);
                b.Property(p => p.ModalityId).IsRequired(true);
                b.Property(p => p.ProjectStatusId).IsRequired(true);
            });

            builder.Entity<TenderBasic>(b =>
            {
                b.ToTable("TenderBasics");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();
                b.Property(p => p.TenderStageId).IsRequired(true);
                b.Property(p => p.SectorId).IsRequired(true);
                b.Property(p => p.PrimaryInchargeId).IsRequired(true);
                b.Property(p => p.Title).IsRequired(true);
                b.Property(p => p.ProjectTitle).IsRequired(false);
                b.Property(p => p.ClientId).IsRequired(true);
                b.Property(p => p.BudgetCurrencyId).IsRequired(true);
                b.Property(p => p.Budget).IsRequired(true);
                b.Property(p => p.DocumentCostCurrencyId).IsRequired(false);
                b.Property(p => p.DocumentCost).IsRequired(false);
                b.Property(p => p.BankGuranteeValueCurrencyId).IsRequired(false);
                b.Property(p => p.BankGuranteeValue).IsRequired(false);
                b.Property(p => p.Department).IsRequired(true);
                b.Property(p => p.Note).IsRequired(true);
                b.Property(p => p.OfficialInvitation).IsRequired(true);
            });

            builder.Entity<ReferenceOthers>(b =>
            {
                b.ToTable("ReferenceOthers");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();

                b.Property(p => p.Title).IsRequired(true);
                b.Property(p => p.Description).IsRequired(true);
            });



            builder.Entity<ContactLabel>(b =>
            {
                b.ToTable("ContactLabels");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();

            });
            builder.Entity<City>(b =>
            {
                b.ToTable("Cities");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();

            });
            builder.Entity<Country>(b =>
            {
                b.ToTable("Countries");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();

            });

            builder.Entity<ContactAddress>(b =>
            {
                b.ToTable("ContactAddresses");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();

                b.Property(p => p.IsActive).IsRequired(true);
                b.Property(p => p.IsPrimaryAddress).IsRequired(true);
                b.Property(p => p.PoBox).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.PostalCodeZip).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.StateProvince).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.StreetName).IsRequired(false).HasMaxLength(400);

            });

            builder.Entity<ContactCompany>(b =>
            {
                b.ToTable("ContactCompanies");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();

                b.Property(p => p.OldId).IsRequired(true);
                b.Property(p => p.IsActive).IsRequired(true);
                b.Property(p => p.CompanyName).IsRequired(true).HasMaxLength(400);
                b.Property(p => p.CompanyAbbrevation).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.DefaultThrough).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.Notes).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.ScopeOfWork).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.MailAddress).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.CompanyCEO).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.IsQuickEntry).IsRequired(true).HasMaxLength(400);
                b.Property(p => p.TagNames).IsRequired(false).HasMaxLength(400);
            });


            builder.Entity<ContactLabelData>(b =>
            {
                b.ToTable("ContactLabelDatas");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();

                b.Property(p => p.IsActive).IsRequired(true);
                b.Property(p => p.LabelData).IsRequired(true).HasMaxLength(400);

            });


            builder.Entity<ContactPerson>(b =>
            {
                b.ToTable("ContactPeople");
                b.ConfigureConcurrencyStamp();
                b.ConfigureExtraProperties();
                b.ConfigureAudited();
                b.ConfigureByConvention();

                b.Property(p => p.IsActive).IsRequired(true);
                b.Property(p => p.Suffix).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.FirstName).IsRequired(true).HasMaxLength(400);
                b.Property(p => p.MiddleName).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.LastName).IsRequired(true).HasMaxLength(400);
                b.Property(p => p.NickName).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.PersonalAbbrevation).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.DateOfBirth).IsRequired(false);
                b.Property(p => p.AnniversaryDate).IsRequired(false);
                b.Property(p => p.SpouseName).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.DesignationName).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.DepartmentName).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.Hobbies).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.Notes).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.DefaultThrough).IsRequired(false).HasMaxLength(400);
                b.Property(p => p.IsQuickEntry).IsRequired(true).HasMaxLength(400);
                b.Property(p => p.TagNames).IsRequired(false).HasMaxLength(400);
            });

            

        }
    }
}