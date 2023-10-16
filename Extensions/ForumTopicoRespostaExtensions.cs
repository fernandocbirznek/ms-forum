using ms_forum.Domains;
using ms_forum.Features.ForumTopicoRespostaFeature.Commands;

namespace ms_forum.Extensions
{
    public static class ForumTopicoRespostaExtensions
    {
        public static ForumTopicoResposta ToDomain(this InserirForumTopicoRespostaCommand request)
        {
            return new()
            {
                Descricao = request.Descricao,
                UsuarioId = request.UsuarioId,
                ForumTopicoId = request.ForumTopicoId,
                DataCadastro = DateTime.Now
            };
        }

        public static ForumTopicoResposta ToUpdate(this ForumTopicoResposta request)
        {
            return new()
            {
                Descricao = request.Descricao,
                DataAtualizacao = DateTime.Now
            };
        }
    }
}
