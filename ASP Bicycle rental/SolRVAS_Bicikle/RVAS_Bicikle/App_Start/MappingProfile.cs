using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using RVAS_Bicikle.DataTransferObjects;
using RVAS_Bicikle.Models;

namespace RVAS_Bicikle.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapiranje Customer klase na CustomerDto i obrnuto preko imena svojstava
            Mapper.CreateMap<Customer, CustomerDto>();
            Mapper.CreateMap<CustomerDto, Customer>();
        }
        
    }
}