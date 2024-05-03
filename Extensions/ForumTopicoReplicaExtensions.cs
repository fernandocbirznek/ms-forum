using ms_forum.Domains;
using ms_forum.Features.ForumTopicoReplicaFeature.Commands;

namespace ms_forum.Extensions
{
    public static class ForumTopicoReplicaExtensions
    {
        public static ForumTopicoReplica ToDomain(this InserirForumTopicoReplicaCommand request)
        {
            return new()
            {
                Descricao = request.Descricao,
                UsuarioId = request.UsuarioId,
                ForumTopicoRespostaId = request.ForumTopicoRespostaId,
                ForumTopicoId = request.ForumTopicoId,
                DataCadastro = DateTime.Now
            };
        }

        public static ForumTopicoReplica ToUpdate(this ForumTopicoReplica request)
        {
            return new()
            {
                Descricao = request.Descricao,
                DataAtualizacao = DateTime.Now
            };
        }
    }
}
