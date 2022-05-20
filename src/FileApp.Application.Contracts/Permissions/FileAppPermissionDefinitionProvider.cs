using FileApp.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace FileApp.Permissions
{
    public class FileAppPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(FileAppPermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(FileAppPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<FileAppResource>(name);
        }
    }
}
