using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoFeature.Queries
{
    public class SelecionarForumTopicoFiltersQuery : IRequest<IEnumerable<SelecionarForumTopicoFiltersQueryResponse>>
    {
    }

    public class SelecionarForumTopicoFiltersQueryResponse : Entity
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public IEnumerable<ForumTag> Tags { get; set; }
        public long ForumId { get; set; }
    }

    public class SelecionarForumTopicoFiltersQueryResponseHandler : IRequestHandler<SelecionarForumTopicoFiltersQuery, IEnumerable<SelecionarForumTopicoFiltersQueryResponse>>
    {
        private readonly IRepository<ForumTopico> _repository;

        public SelecionarForumTopicoFiltersQueryResponseHandler
        (
            IRepository<ForumTopico> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarForumTopicoFiltersQueryResponse>> Handle
        (
            SelecionarForumTopicoFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoFiltersQuery>());

            IEnumerable<ForumTopico> forumMany = await _repository.GetAsync(cancellationToken, item => item.ForumTopicoTags);

            List<SelecionarForumTopicoFiltersQueryResponse> responseMany = new List<SelecionarForumTopicoFiltersQueryResponse>();

            foreach (ForumTopico forum in forumMany)
            {
                SelecionarForumTopicoFiltersQueryResponse response = new SelecionarForumTopicoFiltersQueryResponse();
                response.Titulo = forum.Titulo;
                response.Descricao = forum.Descricao;
                response.UsuarioId = forum.UsuarioId;
                response.ForumId = forum.ForumId;
                response.DataCadastro = forum.DataCadastro;
                response.DataAtualizacao = forum.DataAtualizacao;
                response.Id = forum.Id;
                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
