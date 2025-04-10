using AutoMapper;
using PlataformaFbj.DTOs.Usuario;
using PlataformaFbj.DTOs.Jogo;
using PlataformaFbj.DTOs.Feedback;
using PlataformaFbj.Models;

namespace PlataformaFbj.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, DesenvolvedorDto>();
            CreateMap<Usuario, UsuarioTestersDto>();
            CreateMap<UsuarioCadastroDto, Usuario>();
            CreateMap<UsuarioAtualizacaoDto, Usuario>();

            CreateMap<Jogo, JogoDto>();
            CreateMap<Feedback, FeedbackDto>();
        }
    }
}