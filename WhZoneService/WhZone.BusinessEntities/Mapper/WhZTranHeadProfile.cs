using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Mapper;

public class WhZTranHeadProfile : Profile
{
    public WhZTranHeadProfile()
    {
        this.CreateMap<WhZReceivingTranHeadDto, OlcWhztranhead>()
            .ForMember(dest => dest.Fromwhzid, opt => opt.Ignore())
            .ForMember(dest => dest.Towhz, opt => opt.Ignore())
            .ForMember(dest => dest.Pordid, opt => opt.Ignore())
            .ForMember(dest => dest.Pord, opt => opt.Ignore())
            .ForMember(dest => dest.Sordid, opt => opt.Ignore())
            .ForMember(dest => dest.Sord, opt => opt.Ignore())
            .ForMember(dest => dest.Taskid, opt => opt.Ignore())
            .ForMember(dest => dest.Gen, opt => opt.Ignore())
            .ForMember(dest => dest.Closeusr, opt => opt.Ignore())
            .ForMember(dest => dest.Fromwhz, opt => opt.Ignore())
            .ForMember(dest => dest.St, opt => opt.Ignore())
            .ForMember(dest => dest.Cmp, opt => opt.Ignore())
            .ForMember(dest => dest.OlcWhztranlines, opt => opt.Ignore())
            .ForMember(dest => dest.OlcWhzlocs, opt => opt.Ignore())
            .ForMember(dest => dest.Addusrid, opt => opt.Ignore())
            .ForMember(dest => dest.Addusr, opt => opt.Ignore())
            .ForMember(dest => dest.Adddate, opt => opt.Ignore());

        this.CreateMap<OlcWhztranhead, WhZReceivingTranHeadDto>();
    }
}
