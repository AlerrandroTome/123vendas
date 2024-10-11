using _123vendas.Domain.DTOs;
using _123vendas.Domain.Entities.Aggregates.Sales;
using AutoMapper;

namespace _123vendas.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SaleDto, Sale>()
                .ConstructUsing(src => new Sale(src.CustomerId, src.CustomerName, src.BranchId, src.BranchName))
                .ReverseMap();

            CreateMap<SaleItemDto, SaleItem>()
                .ReverseMap();
        }
    }
}