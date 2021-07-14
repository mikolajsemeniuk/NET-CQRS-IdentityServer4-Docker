using AutoMapper;
using customer.write.Entities;
using customer.write.Inputs;

namespace customer.write.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerInput, CustomerEntity>();           
        }
    }
}