
using System.Globalization;
using AutoMapper;
using Google.Cloud.Firestore;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Application.DTOs.Food;
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
            CreateMap<Post, CreatePostDto>()
             .ForMember(dest => dest.FormattedDate, opt => opt.MapFrom(src => src.Date.ToDateTime().ToString("yyyy-MM-dd")));

            // CreatePostDto -> Post
            CreateMap<CreatePostDto, Post>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.ParseExact(src.FormattedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToUniversalTime())));

            // Post -> UpdatePostDto

            CreateMap<Post, UpdatePostDto>()
             .ForMember(dest => dest.FormattedDate, opt => opt.MapFrom(src => src.Date.ToDateTime().ToString("yyyy-MM-dd")));

            // CreatePostDto -> Post
            CreateMap<UpdatePostDto, Post>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => Timestamp.FromDateTime(DateTime.ParseExact(src.FormattedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToUniversalTime())));

            // Food <-> FoodDto
            CreateMap<Food, FoodDto>().ReverseMap();

            // Food -> CreateFoodDto
            CreateMap<Food, CreateFoodDto>().ReverseMap();

            // Food -> UpdateFoodDto
            CreateMap<Food, UpdateFoodDto>().ReverseMap();
        }
    }
}