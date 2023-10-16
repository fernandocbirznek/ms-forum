using ms_forum.Domains;
using ms_forum.Features.ForumFeature.Commands;

namespace ms_forum.Extensions
{
    public static class ForumExtensions
    {
        public static Forum ToDomain(this InserirForumCommand request)
        {
            return new()
            {
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                DataCadastro = DateTime.Now
            };
        }

        public static Forum ToUpdate(this Forum request)
        {
            return new()
            {
                Id = request.Id,
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                DataCadastro = request.DataCadastro,
                DataAtualizacao = DateTime.Now
            };
        }
    }
}
