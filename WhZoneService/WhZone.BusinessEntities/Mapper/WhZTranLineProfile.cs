using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Mapper;

public class WhZTranLineProfile : Profile
{
    public WhZTranLineProfile()
    {
        this.CreateMap<WhZReceivingTranLineDto, OlcWhztranline>()
            .ForMember(dest => dest.Item, opt => opt.Ignore())
            .ForMember(dest => dest.Stline, opt => opt.Ignore())
            .ForMember(dest => dest.Pordline, opt => opt.Ignore())
            .ForMember(dest => dest.Sordline, opt => opt.Ignore())
            .ForMember(dest => dest.Stline, opt => opt.Ignore())
            .ForMember(dest => dest.Unitid2Navigation, opt => opt.Ignore())
            .ForMember(dest => dest.Whzt, opt => opt.Ignore());

        this.CreateMap<OlcWhztranline, WhZReceivingTranLineDto>();
    }
}
