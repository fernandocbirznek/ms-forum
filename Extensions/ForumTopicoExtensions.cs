using ms_forum.Domains;
using ms_forum.Features.ForumTopicoFeature.Commands;

namespace ms_forum.Extensions
{
    public static class ForumTopicoExtensions
    {
        public static ForumTopico ToDomain(this InserirForumTopicoCommand request)
        {
            return new()
            {
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                UsuarioId = request.UsuarioId,
                ForumId = request.ForumId,
                DataCadastro = DateTime.Now
            };
        }

        public static ForumTopico ToUpdate(this ForumTopico request)
        {
            return new()
            {
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                UsuarioId = request.UsuarioId,
                ForumId = request.ForumId,
                DataAtualizacao = DateTime.Now
            };
        }
    }
}
