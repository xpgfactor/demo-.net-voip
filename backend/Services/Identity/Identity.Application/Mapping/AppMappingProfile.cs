using AutoMapper;
using Identity.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Mapping
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<AuthenticateRequest, IdentityUser>().ReverseMap();
        }
    }
}