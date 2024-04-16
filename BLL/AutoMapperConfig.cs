using AutoMapper;
using DAL;

namespace BLL
{
    public class AutoMapperConfig
    {
        public static IMapper? Mapper { get; private set; }
        public static void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserDTO, User>();
                cfg.CreateMap<Organisation, OrganisationDTO>();
                cfg.CreateMap<OrganisationDTO, Organisation>();
            });

            Mapper = config.CreateMapper();

        }
    }
}
