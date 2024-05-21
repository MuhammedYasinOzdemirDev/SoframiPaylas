using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Cloud.Firestore;
using SoframiPaylas.Application.DTOs;
using SoframiPaylas.Application.DTOs.Post;
using SoframiPaylas.Application.DTOs.User;
using SoframiPaylas.WebUI.Models;

namespace SoframiPaylas.WebUI.Mappings
{
    public class ViewModelToDtoProfile : Profile
    {
        public ViewModelToDtoProfile()
        {
            // ViewModel'den DTO'ya dönüşüm
            CreateMap<PostViewModel, PostDto>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UserProfileViewModel, UserDto>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ChangePasswordRequest, ChangeUserPasswordDto>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UserProfileViewModel, UpdateUserDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.IsHost, opt => opt.MapFrom(src => src.IsHost))
            .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
            .ForMember(dest => dest.About, opt => opt.MapFrom(src => src.About))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role)) // Default value for Role
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone));

            CreateMap<RegisterViewModel, CreateUserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.IsHost, opt => opt.MapFrom(src => false)) // Default değer
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => "https://example.com/images/default-profile.jpg")) // Default URL
                .ForMember(dest => dest.About, opt => opt.MapFrom(src => "")) // Boş string olarak başlatılıyor
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => "user")) // Default role
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname)).ForSourceMember(src => src.Password, opt => opt.DoNotValidate()) // Password dönüşüme dahil edilmiyor
                .ForSourceMember(src => src.ConfirmPassword, opt => opt.DoNotValidate());

            CreateMap<CreatePostViewModel, PostDto>()
               .ForMember(dest => dest.RelatedFoods, opt => opt.Ignore())
               .ForMember(dest => dest.Participants, opt => opt.Ignore()).
               ForMember(dest => dest.Location, opt => opt.Ignore()).
               ForMember(dest => dest.FormattedDate, opt => opt.MapFrom(src => FormatDate(src.FormattedDate)))
               .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))

            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => FormatTime(src.Time)));

        }
        private string FormatDate(string date)
        {
            if (DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate.ToString("yyyy-MM-dd");
            }
            return date;
        }

        private string FormatTime(string time)
        {
            if (DateTime.TryParseExact(time, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedTime))
            {
                return parsedTime.ToString("HH:mm");
            }
            return time;
        }
    }
}