using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DelegateDecompiler;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Dto;
using eLog.HeavyTools.Services.WhZone.BusinessEntities.Model;

namespace eLog.HeavyTools.Services.WhZone.BusinessEntities.Mapper;

public class WhZStockMapProfile : Profile
{
    public WhZStockMapProfile()
    {
        this.CreateMap<OlcWhzstockmap, WhZStockMapQDto>()
            .ForMember(dest => dest.Whname, opt => opt.MapFrom(e => e.Wh.Name))
            .ForMember(dest => dest.Itemcode, opt => opt.MapFrom(e => e.Item.Itemcode))
            .ForMember(dest => dest.Itemname, opt => opt.MapFrom(e => e.Item.Name01))
            .ForMember(dest => dest.Whzonecode, opt => opt.MapFrom(e => e.Whzone!.Whzonecode))
            .ForMember(dest => dest.Whzonename, opt => opt.MapFrom(e => e.Whzone!.Name))
            .ForMember(dest => dest.Whloccode, opt => opt.MapFrom(e => e.Whloc.Whloccode))
            .ForMember(dest => dest.Whlocname, opt => opt.MapFrom(e => e.Whloc.Name))
            .ForMember(dest => dest.Freeqty, opt => opt.MapFrom<FreeQtyResolver>());
    }

    class FreeQtyResolver : IValueResolver<OlcWhzstockmap, WhZStockMapQDto, decimal>
    {
        public decimal Resolve(OlcWhzstockmap source, WhZStockMapQDto destination, decimal destMember, ResolutionContext context)
        {
            return source.Actqty - source.Resqty;
        }
    }
}
