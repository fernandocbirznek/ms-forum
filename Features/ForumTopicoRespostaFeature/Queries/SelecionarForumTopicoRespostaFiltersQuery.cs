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

        public string UsuarioNome { get; set; }
        public byte[]? UsuarioFoto { get; set; }
    }

    public class SelecionarForumRespostaFiltersQueryResponseHandler : IRequestHandler<SelecionarForumTopicoRespostaFiltersQuery, IEnumerable<SelecionarForumTopicoRespostaFiltersQueryResponse>>
    {
        private readonly IRepository<ForumTopicoResposta> _repository;
        private readonly IUsuarioService _usuarioService;

        public SelecionarForumRespostaFiltersQueryResponseHandler
        (
            IRepository<ForumTopicoResposta> repository,
            IUsuarioService usuarioService
        )
        {
            _repository = repository;
            _usuarioService = usuarioService;
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

                var usuario = await _usuarioService.GetUsuarioByIdAsync(forumTopicoResposta.UsuarioId);
                if (usuario is null)
                {
                    usuario = new Service.UsuarioService.UsuarioResponse();
                }

                response.UsuarioNome = usuario.Nome;
                response.UsuarioFoto = usuario.Foto;

                responseMany.Add(response);
            }

            return responseMany;
        }
    }
}
