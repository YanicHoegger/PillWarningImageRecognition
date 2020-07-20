using AutoMapper;
using Clients.Shared;
using DatabaseInteraction.Interface;

namespace WebInterface.Services
{
    public static class Converter
    {
        private static readonly Mapper _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DrugCheckingSource, PillWarning>();
            cfg.CreateMap<DrugCheckingInfo, PillWarningInfo>();
        }));

        public static PillWarning ToPillWarning(DrugCheckingSource drugCheckingSource)
        {
            return _mapper.Map<PillWarning>(drugCheckingSource);
        }
    }
}
