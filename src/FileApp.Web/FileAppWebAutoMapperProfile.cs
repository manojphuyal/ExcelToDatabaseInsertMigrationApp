using AutoMapper;
using FileApp.CqNovelImport.ImportContact;

namespace FileApp.Web
{
    public class FileAppWebAutoMapperProfile : Profile
    {
        public FileAppWebAutoMapperProfile()
        {
            //Define your AutoMapper configuration here for the Web project.
            CreateMap<ContactCompanyDto, ContactCompanyAddUpdateDto>();

        }
    }
}
