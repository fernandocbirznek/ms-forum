using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoRespostaFeature.Queries
{
    public class SelecionarForumTopicoRespostaFiltersQuery : IRequest<IEnumerable<SelecionarForumTopicoRespostaFiltersQueryResponse>>
    {
    }

    public class SelecionarForumTopicoRespostaFiltersQueryResponse : Entity
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
    }

    public class SelecionarForumRespostaFiltersQueryResponseHandler : IRequestHandler<SelecionarForumTopicoRespostaFiltersQuery, IEnumerable<SelecionarForumTopicoRespostaFiltersQueryResponse>>
    {
        private readonly IRepository<ForumTopicoResposta> _repository;

        public SelecionarForumRespostaFiltersQueryResponseHandler
        (
            IRepository<ForumTopicoResposta> repository
        )
        {
            _repository = repository;
        }

        public async Task<IEnumerable<SelecionarForumTopicoRespostaFiltersQueryResponse>> Handle
        (
            SelecionarForumTopicoRespostaFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoRespostaFiltersQuery>());

            IEnumerable<ForumTopicoResposta> forumTopicoRespostaMany = await _repository.GetAsync(cancellationToken);

            List<SelecionarForumTopicoRespostaFiltersQueryResponse> responseMany = new List<SelecionarForumTopicoRespostaFiltersQueryResponse>();

            foreach (ForumTopicoResposta forumTopicoResposta in forumTopicoRespostaMany)
            {
                SelecionarForumTopicoRespostaFiltersQueryResponse response = new SelecionarForumTopicoRespostaFiltersQueryResponse();

                response.Descricao = forumTopicoResposta.Descricao;
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
