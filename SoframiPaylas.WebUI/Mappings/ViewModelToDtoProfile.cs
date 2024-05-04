using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

using SoframiPaylas.WebUI.Models;

namespace SoframiPaylas.WebUI.Mappings
{
    public class ViewModelToDtoProfile : Profile
    {
        public ViewModelToDtoProfile()
        {
            // ViewModel'den DTO'ya dönüşüm
            CreateMap<EventViewModel, EventDto>().ReverseMap().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}