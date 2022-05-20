using System.Threading.Tasks;
using FileApp.Localization;
using FileApp.MultiTenancy;
using Volo.Abp.Identity.Web.Navigation;
using Volo.Abp.SettingManagement.Web.Navigation;
using Volo.Abp.TenantManagement.Web.Navigation;
using Volo.Abp.UI.Navigation;

namespace FileApp.Web.Menus
{
    public class FileAppMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var administration = context.Menu.GetAdministration();
            var l = context.GetLocalizer<FileAppResource>();

            context.Menu.Items.Insert(0,new ApplicationMenuItem(FileAppMenus.Home,l["Menu:Home"],"~/",icon: "fas fa-home",order: 0));
            context.Menu.AddItem(
                    new ApplicationMenuItem("FileApp", l["FileApp"], icon: "fa fa-book")
                    .AddItem(new ApplicationMenuItem("FileApp.ContactCompany", l["Contact Company"], url: "/CqNovelImport/ImportContact/ContactCompany"))
                    .AddItem(new ApplicationMenuItem("FileApp.ContactPeople", l["Contact People"], url: "/CqNovelImport/ImportContact/ContactPeople"))
                    .AddItem(new ApplicationMenuItem("FileApp.ContactPeople", l["Contact Labels Datas"], url: "/CqNovelImport/ImportContact/ContactLabelsDatas"))
                    .AddItem(new ApplicationMenuItem("FileApp.ContactAddresses", l["Contact Addresses"], url: "/CqNovelImport/ImportContact/ContactAddresses"))

                     .AddItem(new ApplicationMenuItem("FileApp.Country", l["Country"], url: "/CqNovelImport/ImportContact/Country"))
                    .AddItem(new ApplicationMenuItem("FileApp.City", l["City"], url: "/CqNovelImport/ImportContact/City"))


                    .AddItem(new ApplicationMenuItem("FileApp.OldContactCompany", l["Old Contact Company"], url: "/CqNovelImport/OldImportContact/OldContactCompany"))
                    .AddItem(new ApplicationMenuItem("FileApp.Reference", l["Reference"], url: "/"))
                    .AddItem(new ApplicationMenuItem("FileApp.ReferenceProject", l["Reference Project"], url: "/CqNovelImport/ReferenceImport/ReferenceProject"))
                    .AddItem(new ApplicationMenuItem("FileApp.ReferenceTender", l["Reference Tender"], url: "/CqNovelImport/ReferenceImport/ReferenceTender"))
                    .AddItem(new ApplicationMenuItem("FileApp.ReferenceOther", l["Reference Other"], url: "/CqNovelImport/ReferenceImport/ReferenceOther"))
     );                
            if (MultiTenancyConsts.IsEnabled)
            {
                administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
            }
            else
            {
                administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
            }

            administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
            administration.SetSubItemOrder(SettingManagementMenuNames.GroupName, 3);
        }
    }
}
