using System.Threading.Tasks;

namespace FileApp.Data
{
    public interface IFileAppDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
