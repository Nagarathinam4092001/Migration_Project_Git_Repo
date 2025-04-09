using AutoMapper;
using Migration_Project.DTOs;
using Migration_Project.Models;
using System;

namespace Migration_Project.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Define a mapping between Customer and CustomerDto
            CreateMap<Customer, CustomerDTO>().ReverseMap();
        }
    }
}
