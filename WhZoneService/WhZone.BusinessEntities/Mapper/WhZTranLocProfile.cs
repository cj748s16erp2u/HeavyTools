using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Mapper;

public class WhZTranLocProfile : Profile
{
    public WhZTranLocProfile()
    {
        this.CreateMap<WhZTranLocDto, OlcWhztranloc>()
            .ForMember(dest => dest.Whzt, opt => opt.Ignore())
            .ForMember(dest => dest.Whztline, opt => opt.Ignore())
            .ForMember(dest => dest.Wh, opt => opt.Ignore())
            .ForMember(dest => dest.Whzone, opt => opt.Ignore())
            .ForMember(dest => dest.Whloc, opt => opt.Ignore());

        this.CreateMap<OlcWhztranloc, WhZTranLocDto>()
            .ForMember(dest => dest.Whname, opt => opt.MapFrom(s => s.Wh.Name))
            .ForMember(dest => dest.Whzonecode, opt => opt.MapFrom(s => s.Whzone.Whzonecode))
            .ForMember(dest => dest.Whzonename, opt => opt.MapFrom(s => s.Whzone.Name))
            .ForMember(dest => dest.Whloccode, opt => opt.MapFrom(s => s.Whloc.Whloccode))
            .ForMember(dest => dest.Whlocname, opt => opt.MapFrom(s => s.Whloc.Name))
            .ForMember(dest => dest.Itemid, opt => opt.MapFrom(s => s.Whztline.Itemid))
            .ForMember(dest => dest.Itemcode, opt => opt.MapFrom(s => s.Whztline.Item.Itemcode))
            .ForMember(dest => dest.Itemname01, opt => opt.MapFrom(s => s.Whztline.Item.Name01));
    }
}
