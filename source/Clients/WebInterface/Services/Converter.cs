using AutoMapper;
using Clients.Shared;
using Domain.Interface;

namespace WebInterface.Services
{
    public static class Converter
    {
        private static readonly Mapper _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<IPredictionResult, PredictionResult>();
            cfg.CreateMap<IFinding, Finding>();
            cfg.CreateMap<IPillWarning, PillWarning>();
            cfg.CreateMap<IPillWarningInfo, PillWarningInfo>();
        }));

        public static PredictionResult ToPredictionResult(IPredictionResult pillWarning)
        {
            return _mapper.Map<PredictionResult>(pillWarning);
        }
    }
}
