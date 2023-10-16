using ms_forum.Domains;
using ms_forum.Features.ForumTagFeature.Commands;

namespace ms_forum.Extensions
{
    public static class ForumTagExtensions
    {
        public static ForumTag ToDomain(this InserirForumTagCommand request)
        {
            return new()
            {
                Titulo = request.Titulo,
                DataCadastro = DateTime.Now
            };
        }

        public static ForumTag ToUpdate(this ForumTag request)
        {
            return new()
            {
                Titulo = request.Titulo,
                DataCadastro = request.DataCadastro,
                DataAtualizacao = DateTime.Now
            };
        }
    }
}
