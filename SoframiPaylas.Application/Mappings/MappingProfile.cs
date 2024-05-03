using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Application.DTOs.Event;
using SoframiPaylas.Application.DTOs.Post;
using SoframiPaylas.Domain.Entities;

namespace SoframiPaylas.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            // User <-> UserDto
            CreateMap<User, UserDto>()
                .ReverseMap();

            // User -> CreateUserDto
            CreateMap<User, CreateUserDto>()
                .ReverseMap();

            // User -> UpdateUserDto
            CreateMap<User, UpdateUserDto>()
                .ReverseMap();
        }
    }
}