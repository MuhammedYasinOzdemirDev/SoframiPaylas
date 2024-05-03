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

            // Post <-> PostDto
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.FormattedDate, opt => opt.MapFrom(src => src.Date.ToDateTime().ToString("yyyy-MM-dd"))); // Tarih formatlaması

            // Post -> CreatePostDto
            CreateMap<Post, CreatePostDto>().ReverseMap();

            // Post -> UpdatePostDto
            CreateMap<Post, UpdatePostDto>().ReverseMap();

            // Event <-> EventDto
            CreateMap<Event, EventDto>().ReverseMap();

            // Event -> CreateEventDto
            CreateMap<Event, CreateEventDto>().ReverseMap();

            // Event -> UpdateEventDto
            CreateMap<Event, UpdateEventDto>().ReverseMap();
        }
    }
}