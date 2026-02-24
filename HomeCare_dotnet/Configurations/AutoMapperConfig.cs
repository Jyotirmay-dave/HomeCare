using System;
using AutoMapper;
using HomeCare_dotnet.Data;
using HomeCare_dotnet.DTOs;

namespace HomeCare_dotnet.Configurations;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Admin, AdminCreateDTO>().ReverseMap();
        CreateMap<Admin, AdminLoginDTO>().ReverseMap();
        CreateMap<Admin, AdminDTO>().ReverseMap();
        CreateMap<User, UserLoginDTO>().ReverseMap();
        CreateMap<User, UserDTO>().ReverseMap();
    }
}
