using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTagFeature.Queries
{
    public class SelecionarForumTagFiltersQuery : IRequest<IEnumerable<SelecionarForumTagFiltersQueryResponse>>
    {
    }

    public class SelecionarForumTagFiltersQueryResponse : Entity
    {
        public string Titulo { get; set; }
    }

    public class SelecionarForumTagFiltersQueryResponseHandler : IRequestHandler<SelecionarForumTagFiltersQuery, IEnumerable<SelecionarForumTagFiltersQueryResponse>>
    {
        private readonly IRepository<ForumTag> _repository;

        public SelecionarForumTagFiltersQueryResponseHandler
        (
            IRepository<ForumTag> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarForumTagFiltersQueryResponse>> Handle
        (
            SelecionarForumTagFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTagFiltersQuery>());

            IEnumerable<ForumTag> forumMany = await _repository.GetAsync(cancellationToken);

            List<SelecionarForumTagFiltersQueryResponse> responseMany = new List<SelecionarForumTagFiltersQueryResponse>();

            foreach (ForumTag forum in forumMany)
            {
                SelecionarForumTagFiltersQueryResponse response = new SelecionarForumTagFiltersQueryResponse();
                response.Titulo = forum.Titulo;
                response.DataCadastro = forum.DataCadastro;
                response.DataAtualizacao = forum.DataAtualizacao;
                response.Id = forum.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
