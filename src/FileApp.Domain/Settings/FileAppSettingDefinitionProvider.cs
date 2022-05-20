using Volo.Abp.Settings;

namespace FileApp.Settings
{
    public class FileAppSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(FileAppSettings.MySetting1));
        }
    }
}
