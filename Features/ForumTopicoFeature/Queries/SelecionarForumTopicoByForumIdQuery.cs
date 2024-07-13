using MediatR;
using ms_forum.Domains;
using ms_forum.Enum;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoFeature.Queries
{
    public class SelecionarForumTopicoByForumIdQuery : IRequest<IEnumerable<SelecionarForumTopicoByForumIdQueryResponse>>
    {
        public long Id { get; set; }
    }

    public class SelecionarForumTopicoByForumIdQueryResponse : Entity
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public ForumTopicoEnum ForumTopicoEnum { get; set; }
        public IEnumerable<ForumTag> ForumTagMany { get; set; }
        public long ForumId { get; set; }
        public long Resposta {  get; set; }
    }

    public class SelecionarForumTopicoByForumIdQueryHandler : 
        IRequestHandler<SelecionarForumTopicoByForumIdQuery, IEnumerable<SelecionarForumTopicoByForumIdQueryResponse>>
    {
        private readonly IRepository<ForumTopico> _repository;
        private readonly IRepository<ForumTopicoResposta> _repositoryForumTopicoResposta;
        private readonly IRepository<ForumTag> _repositoryForumTag;

        public SelecionarForumTopicoByForumIdQueryHandler
        (
            IRepository<ForumTopico> repository,
            IRepository<ForumTopicoResposta> repositoryForumTopicoResposta,
            IRepository<ForumTag> repositoryForumTag
        )
        {
            _repository = repository;
            _repositoryForumTopicoResposta = repositoryForumTopicoResposta;
            _repositoryForumTag = repositoryForumTag;
        }

        public async Task<IEnumerable<SelecionarForumTopicoByForumIdQueryResponse>> Handle
        (
            SelecionarForumTopicoByForumIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoByForumIdQuery>());

            IEnumerable<ForumTopico> forumTopicoMany = await GetAsync(request, cancellationToken);

            IEnumerable<ForumTag> forumTagMany = await GetForumTagAsync(cancellationToken);

            List<SelecionarForumTopicoByForumIdQueryResponse> responseMany = new List<SelecionarForumTopicoByForumIdQueryResponse>();

            foreach (ForumTopico forumTopico in forumTopicoMany)
            {
                List<ForumTag> forumTopicoTagMany = new List<ForumTag>();
                foreach (ForumTopicoTag forumTopicoTag in forumTopico.ForumTopicoTags)
                {
                    ForumTag forumTag = forumTagMany.First(item => item.Id.Equals(forumTopicoTag.ForumTagId));
                    if (forumTag is not null)
                        forumTopicoTagMany.Add(forumTag);
                }

                var forumTopicoRespostaMany = await GetForumTopicoRespostaCountAsync(forumTopico.Id, cancellationToken);

                SelecionarForumTopicoByForumIdQueryResponse response = new SelecionarForumTopicoByForumIdQueryResponse();
                response.Titulo = forumTopico.Titulo;
                response.Descricao = forumTopico.Descricao;
                response.UsuarioId = forumTopico.UsuarioId;
                response.ForumId = forumTopico.ForumId;
                response.ForumTagMany = forumTopicoTagMany;
                response.ForumTopicoEnum = forumTopico.ForumTopicoEnum;
                response.DataCadastro = forumTopico.DataCadastro;
                response.DataAtualizacao = forumTopico.DataAtualizacao;
                response.Id = forumTopico.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }

        private async Task<IEnumerable<ForumTopico>> GetAsync
        (
            SelecionarForumTopicoByForumIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetAsync
                (
                    item => item.ForumId.Equals(request.Id),
                    cancellationToken,
                    item => item.ForumTopicoTags
                );
        }

        private async Task<IEnumerable<ForumTag>> GetForumTagAsync
        (
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTag.GetAsync
                (
                    cancellationToken
                );
        }

        private async Task<IEnumerable<ForumTopicoResposta>> GetForumTopicoRespostaCountAsync
        (
            long request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTopicoResposta.GetAsync
                (
                    item => item.ForumTopicoId.Equals(request),
                    cancellationToken
                );
        }
    }
}
