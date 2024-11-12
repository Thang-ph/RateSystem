using AutoMapper;
using ProjectAPI.Models;
namespace ProjectAPI.Mappers
{
    public class MapperProduct : Profile
    {
        public MapperProduct()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
            CreateMap<Category, CategoryDTO>();
            CreateMap<Comment, CommentDTO>()
             .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User)); 
            CreateMap<User, UserDTO>();
        }
    }
}
