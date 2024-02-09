using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NoobChessServer.DTOs.UserDtos;

namespace NoobChessServer
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // CreateMap<FromSource, ToDestination>
            CreateMap<GoogleUser, User>();
            CreateMap<User, GetUserDto>();
        }
    }
}