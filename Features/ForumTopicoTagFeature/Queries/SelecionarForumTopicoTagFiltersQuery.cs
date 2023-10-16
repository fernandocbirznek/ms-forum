using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoTagFeature.Queries
{
    public class SelecionarForumTopicoTagFiltersQuery : IRequest<IEnumerable<SelecionarForumTopicoTagFiltersQueryResponse>>
    {
    }

    public class SelecionarForumTopicoTagFiltersQueryResponse : Entity
    {
        public string TagTitulo { get; set; }
        public string ForumTopicoTitulo { get; set; }
    }

    public class SelecionarForumTopicoTagFiltersQueryResponseHandler : IRequestHandler<SelecionarForumTopicoTagFiltersQuery, IEnumerable<SelecionarForumTopicoTagFiltersQueryResponse>>
    {
        private readonly IRepository<ForumTopicoTag> _repository;

        public SelecionarForumTopicoTagFiltersQueryResponseHandler
        (
            IRepository<ForumTopicoTag> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarForumTopicoTagFiltersQueryResponse>> Handle
        (
            SelecionarForumTopicoTagFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoTagFiltersQuery>());

            IEnumerable<ForumTopicoTag> forumMany = await _repository.GetAsync
                (
                    cancellationToken,
                    item => item.ForumTag,
                    item => item.ForumTopico
                );

            List<SelecionarForumTopicoTagFiltersQueryResponse> responseMany = new List<SelecionarForumTopicoTagFiltersQueryResponse>();

            foreach (ForumTopicoTag forum in forumMany)
            {
                SelecionarForumTopicoTagFiltersQueryResponse response = new SelecionarForumTopicoTagFiltersQueryResponse();
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
