using EasyGroceries.Order.Application.DTOs;
using EasyGroceries.Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace EasyGroceries.Order.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderHeaderDto, CartHeaderDto>()
            .ForMember(dest => dest.CartTotal, u => u.MapFrom(src => src.OrderTotal))
            .ForMember(dest => dest.CartHeaderId, u => u.MapFrom(src => src.OrderHeaderId)).ReverseMap();

            CreateMap<CartDetailsDto, OrderDetailsDto>()
            .ForMember(dest => dest.OrderHeaderId, u => u.MapFrom(src => src.CartHeaderId))
            .ForMember(dest => dest.OrderDetailsId, u => u.MapFrom(src => src.CartDetailsId));

            CreateMap<OrderDetailsDto, CartDetailsDto>();

            CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();

            CreateMap<OrderDetailsDto, OrderDetails>()
                .ForMember(dest => dest.Price, u => u.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.Count, u => u.MapFrom(src => src.Product.Count));

            CreateMap<OrderDetails, OrderDetailsDto>();
        }
    }
}
