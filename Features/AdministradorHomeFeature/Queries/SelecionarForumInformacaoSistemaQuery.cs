using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.AdministradorHomeFeature.Queries
{
    public class SelecionarForumInformacaoSistemaQuery
    : IRequest<SelecionarForumInformacaoSistemaQueryResponse>
    {
    }

    public class SelecionarForumInformacaoSistemaQueryResponse
    {
        public long ForumCount { get; set; }
        public long ForumTagCount { get; set; }
        public long ForumTopicoCount { get; set; }
        public long ForumTopicoReplicaCount { get; set; }
        public long ForumTopicoRespostaCount { get; set; }
    }

    public class SelecionarForumInformacaoSistemaQueryHandler
        : IRequestHandler<SelecionarForumInformacaoSistemaQuery,
            SelecionarForumInformacaoSistemaQueryResponse>
    {
        private readonly IRepository<Forum> _repository;
        private readonly IRepository<ForumTag> _repositoryForumTag;
        private readonly IRepository<ForumTopico> _repositoryForumTopico;
        private readonly IRepository<ForumTopicoReplica> _repositoryForumTopicoReplica;
        private readonly IRepository<ForumTopicoResposta> _repositoryForumTopicoResposta;

        public SelecionarForumInformacaoSistemaQueryHandler
        (
            IRepository<Forum> repository,
            IRepository<ForumTag> repositoryForumTag,
            IRepository<ForumTopico> repositoryForumTopico,
            IRepository<ForumTopicoReplica> repositoryForumTopicoReplica,
            IRepository<ForumTopicoResposta> repositoryForumTopicoResposta
        )
        {
            _repository = repository;
            _repositoryForumTag = repositoryForumTag;
            _repositoryForumTopico = repositoryForumTopico;
            _repositoryForumTopicoReplica = repositoryForumTopicoReplica;
            _repositoryForumTopicoResposta = repositoryForumTopicoResposta;
        }

        public async Task<SelecionarForumInformacaoSistemaQueryResponse> Handle
        (
            SelecionarForumInformacaoSistemaQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumInformacaoSistemaQuery>());

            var response = new SelecionarForumInformacaoSistemaQueryResponse();
            response.ForumCount = await _repository.CountAsync(cancellationToken);
            response.ForumTagCount = await _repositoryForumTag.CountAsync(cancellationToken);
            response.ForumTopicoCount = await _repositoryForumTopico.CountAsync(cancellationToken);
            response.ForumTopicoReplicaCount = await _repositoryForumTopicoReplica.CountAsync(cancellationToken);
            response.ForumTopicoRespostaCount = await _repositoryForumTopicoResposta.CountAsync(cancellationToken);

            return response;
        }
    }
}
