using _123vendas.Domain.DTOs;
using _123vendas.Domain.Entities.Aggregates.Sales;
using AutoMapper;

namespace _123vendas.Domain.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateSaleDto, Sale>()
                .AfterMap((src, dest) =>
                {
                    dest.CalculateTotalAmount();
                });
            CreateMap<Sale, CreateSaleDto>();            
            CreateMap<SaleDto, Sale>().ReverseMap();

            CreateMap<SaleItemDto, SaleItem>()
                .AfterMap((src, dest) =>
                {
                    dest.CalculateTotalItemValue();
                });            
            CreateMap<SaleItem, SaleItemDto>();
            CreateMap<CreateSaleItemDto, SaleItem>().ReverseMap();

            CreateMap<UpdateSaleDto, Sale>()
                .AfterMap((src, dest) =>
                {
                    dest.CalculateTotalAmount();
                });
            CreateMap<Sale, UpdateSaleDto>();
            CreateMap<UpdateSaleItemDto, SaleItem>().ReverseMap();
        }
    }
}