using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumFeature.Queries
{
    public class SelecionarForumFiltersQuery : IRequest<IEnumerable<SelecionarForumFiltersQueryResponse>>
    {
    }

    public class SelecionarForumFiltersQueryResponse : Entity
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }

    public class SelecionarForumFiltersQueryResponseHandler : IRequestHandler<SelecionarForumFiltersQuery, IEnumerable<SelecionarForumFiltersQueryResponse>>
    {
        private readonly IRepository<Forum> _repository;

        public SelecionarForumFiltersQueryResponseHandler
        (
            IRepository<Forum> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarForumFiltersQueryResponse>> Handle
        (
            SelecionarForumFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumFiltersQuery>());

            IEnumerable<Forum> forumMany = await _repository.GetAsync(cancellationToken);

            List<SelecionarForumFiltersQueryResponse> responseMany = new List<SelecionarForumFiltersQueryResponse>();

            foreach (Forum forum in forumMany)
            {
                SelecionarForumFiltersQueryResponse response = new SelecionarForumFiltersQueryResponse();
                response.Titulo = forum.Titulo;
                response.Descricao = forum.Descricao;
                response.DataCadastro = forum.DataCadastro;
                response.DataAtualizacao = forum.DataAtualizacao;
                response.Id = forum.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
