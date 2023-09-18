using AutoMapper;
using FilmReview.Models;
using FilmReview.Dto;

namespace FilmReview.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Film, FilmDto>();
            CreateMap<Rating, RatingDto>()
                .ForMember(dest => dest.FilmId, opt => opt.MapFrom(src => src.Film.FilmId));
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.FilmId, opt => opt.MapFrom(src => src.Film.FilmId));
            CreateMap<CategoryDto, Category>();
            CreateMap<CountryDto, Country>();
            CreateMap<FilmDto, Film>();
            CreateMap<RatingDto, Rating>();
           

            CreateMap<UserDto, User>();
            CreateMap<ReviewDto, Review>();
           


        }
    }
}
