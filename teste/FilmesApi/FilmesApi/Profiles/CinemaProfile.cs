using AutoMapper;
using FilmesApi.Controllers;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;

namespace FilmesApi.Profiles
{
    public class CinemaProfile : Profile
    {
        public CinemaProfile() 
        {
            
            CreateMap<CreateCinemaDto, Cinema>();
            //Criando mapeamento de cinema com readcinemadto, aplicação vai converter de cinema para cinemadto;
            //para o campo readenderecodto, pegar da origam (cinema) o campo de endereço e sessoes
            CreateMap<Cinema, ReadCinemaDto>().ForMember(cinemaDto => cinemaDto.Endereco, opt => opt.MapFrom(cinema => cinema.Endereco))
                .ForMember(cinemaDto => cinemaDto.Sessoes, opt => opt.MapFrom(cinema => cinema.Sessoes));
            CreateMap<UpdateCinemaDto, Cinema>();
        }
    }
}
