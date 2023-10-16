using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoReplicaFeature.Queries
{
    public class SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQuery : IRequest<IEnumerable<SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQueryResponse>>
    {
        public long Id { get; set; }
    }

    public class SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQueryResponse : Entity
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoId { get; set; }
        public long ForumTopicoRespostaId { get; set; }
    }

    public class SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQueryResponseHandler : IRequestHandler<SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQuery, IEnumerable<SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQueryResponse>>
    {
        private readonly IRepository<ForumTopicoReplica> _repository;

        public SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQueryResponseHandler
        (
            IRepository<ForumTopicoReplica> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQueryResponse>> Handle
        (
            SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQuery>());

            IEnumerable<ForumTopicoReplica> forumRespostaMany = await _repository.GetAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );

            List<SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQueryResponse> responseMany = new List<SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQueryResponse>();

            foreach (ForumTopicoReplica forumTopicoResposta in forumRespostaMany)
            {
                SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQueryResponse response = new SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQueryResponse();

                response.Descricao = forumTopicoResposta.Descricao;
                response.ForumTopicoRespostaId = forumTopicoResposta.ForumTopicoRespostaId;
                response.UsuarioId = forumTopicoResposta.UsuarioId;
                response.DataCadastro = forumTopicoResposta.DataCadastro;
                response.DataAtualizacao = forumTopicoResposta.DataAtualizacao;
                response.Id = forumTopicoResposta.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
