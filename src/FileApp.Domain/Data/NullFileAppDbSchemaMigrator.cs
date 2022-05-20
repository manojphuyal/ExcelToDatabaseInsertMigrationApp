using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace FileApp.Data
{
    /* This is used if database provider does't define
     * IFileAppDbSchemaMigrator implementation.
     */
    public class NullFileAppDbSchemaMigrator : IFileAppDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}