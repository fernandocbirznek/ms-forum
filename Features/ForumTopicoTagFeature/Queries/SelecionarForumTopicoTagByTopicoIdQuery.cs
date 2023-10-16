using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoTagFeature.Queries
{
    public class SelecionarForumTopicoTagByTopicoIdQuery : IRequest<IEnumerable<SelecionarForumTopicoTagByTopicoIdQueryResponse>>
    {
        public long Id { get; set; }
    }

    public class SelecionarForumTopicoTagByTopicoIdQueryResponse : Entity
    {
        public string TagTitulo { get; set; }
        public string ForumTopicoTitulo { get; set; }
    }

    public class SelecionarForumTopicoTagByTopicoIdQueryResponseHandler : IRequestHandler<SelecionarForumTopicoTagByTopicoIdQuery, IEnumerable<SelecionarForumTopicoTagByTopicoIdQueryResponse>>
    {
        private readonly IRepository<ForumTopicoTag> _repository;

        public SelecionarForumTopicoTagByTopicoIdQueryResponseHandler
        (
            IRepository<ForumTopicoTag> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarForumTopicoTagByTopicoIdQueryResponse>> Handle
        (
            SelecionarForumTopicoTagByTopicoIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoTagByTopicoIdQuery>());

            IEnumerable<ForumTopicoTag> forumMany = await _repository.GetAsync
                (
                    item => item.ForumTopicoId.Equals(request.Id),
                    cancellationToken,
                    item => item.ForumTag,
                    item => item.ForumTopico
                );

            List<SelecionarForumTopicoTagByTopicoIdQueryResponse> responseMany = new List<SelecionarForumTopicoTagByTopicoIdQueryResponse>();

            foreach (ForumTopicoTag forum in forumMany)
            {
                SelecionarForumTopicoTagByTopicoIdQueryResponse response = new SelecionarForumTopicoTagByTopicoIdQueryResponse();
                response.ForumTopicoTitulo = forum.ForumTopico.Titulo;
                response.TagTitulo = forum.ForumTag.Titulo;
                response.DataCadastro = forum.DataCadastro;
                response.DataAtualizacao = forum.DataAtualizacao;
                response.Id = forum.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
