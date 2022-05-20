using AutoMapper;
using FileApp.AppEntities.CqNovelImport.ImportContact;
using FileApp.CqNovelImport.ImportContact;

namespace FileApp
{
    public class FileAppApplicationAutoMapperProfile : Profile
    {
        public FileAppApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<ContactCompany, ContactCompanyDto>();
        }
    }
}
