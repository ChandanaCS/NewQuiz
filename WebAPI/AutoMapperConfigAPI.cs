using AutoMapper;
using BLL;
using WebAPI.Models;

namespace WebAPI
{
    public class AutoMapperConfigAPI
    {
        public static IMapper? Mapper { get; private set; }
        public static void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserDTO, UserModel>();
                cfg.CreateMap<UserModel, UserDTO>();
                cfg.CreateMap<OrganisationDTO, OrganisationModel>();
                cfg.CreateMap<OrganisationModel, OrganisationDTO>();
            });

            Mapper = config.CreateMapper();

        }
    }
}
