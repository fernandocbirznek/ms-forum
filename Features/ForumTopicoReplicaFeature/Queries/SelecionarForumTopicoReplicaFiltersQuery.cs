using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoReplicaFeature.Queries
{
    public class SelecionarForumTopicoReplicaFiltersQuery : IRequest<IEnumerable<SelecionarForumTopicoReplicaFiltersQueryResponse>>
    {
    }

    public class SelecionarForumTopicoReplicaFiltersQueryResponse : Entity
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoId { get; set; }
        public long ForumTopicoRespostaId { get; set; }
    }

    public class SelecionarForumTopicoReplicaFiltersQueryResponseHandler : IRequestHandler<SelecionarForumTopicoReplicaFiltersQuery, IEnumerable<SelecionarForumTopicoReplicaFiltersQueryResponse>>
    {
        private readonly IRepository<ForumTopicoReplica> _repository;

        public SelecionarForumTopicoReplicaFiltersQueryResponseHandler
        (
            IRepository<ForumTopicoReplica> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarForumTopicoReplicaFiltersQueryResponse>> Handle
        (
            SelecionarForumTopicoReplicaFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoReplicaFiltersQuery>());

            IEnumerable<ForumTopicoReplica> forumRespostaMany = await _repository.GetAsync(cancellationToken);

            List<SelecionarForumTopicoReplicaFiltersQueryResponse> responseMany = new List<SelecionarForumTopicoReplicaFiltersQueryResponse>();

            foreach (ForumTopicoReplica forumTopicoResposta in forumRespostaMany)
            {
                SelecionarForumTopicoReplicaFiltersQueryResponse response = new SelecionarForumTopicoReplicaFiltersQueryResponse();

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
