using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SoframiPaylas.Application.DTOs.Post;
using SoframiPaylas.WebUI.Models;

namespace SoframiPaylas.WebUI.Mappings
{
    public class ViewModelToDtoProfile : Profile
    {
        public ViewModelToDtoProfile()
        {
            // ViewModel'den DTO'ya dönüşüm
            CreateMap<PostViewModel, PostDto>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}