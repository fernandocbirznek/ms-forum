using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;
using ms_forum.Service;

namespace ms_forum.Features.ForumTopicoRespostaFeature.Queries
{
    public class SelecionarForumTopicoRespostaByTopicoIdQuery : IRequest<IEnumerable<SelecionarForumTopicoRespostaByTopicoIdQueryResponse>>
    {
        public long Id { get; set; }
    }

    public class SelecionarForumTopicoRespostaByTopicoIdQueryResponse : Entity
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoId { get; set; }

        public string UsuarioNome { get; set; }
        public byte[]? UsuarioFoto { get; set; }
    }

    public class SelecionarForumTopicoRespostaByTopicoIdQueryResponseHandler : IRequestHandler<SelecionarForumTopicoRespostaByTopicoIdQuery, IEnumerable<SelecionarForumTopicoRespostaByTopicoIdQueryResponse>>
    {
        private readonly IRepository<ForumTopicoResposta> _repository;
        private readonly IUsuarioService _usuarioService;

        public SelecionarForumTopicoRespostaByTopicoIdQueryResponseHandler
        (
            IRepository<ForumTopicoResposta> repository,
            IUsuarioService usuarioService
        )
        {
            _repository = repository;
            _usuarioService = usuarioService;
        }

        public async Task<IEnumerable<SelecionarForumTopicoRespostaByTopicoIdQueryResponse>> Handle
        (
            SelecionarForumTopicoRespostaByTopicoIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoRespostaByTopicoIdQuery>());

            IEnumerable<ForumTopicoResposta> forumRespostaMany = await _repository.GetAsync
                (
                    item => item.ForumTopicoId.Equals(request.Id),
                    cancellationToken
                );

            List<SelecionarForumTopicoRespostaByTopicoIdQueryResponse> responseMany = new List<SelecionarForumTopicoRespostaByTopicoIdQueryResponse>();

            foreach (ForumTopicoResposta forumTopicoResposta in forumRespostaMany)
            {
                SelecionarForumTopicoRespostaByTopicoIdQueryResponse response = new SelecionarForumTopicoRespostaByTopicoIdQueryResponse();

                response.Descricao = forumTopicoResposta.Descricao;
                response.UsuarioId = forumTopicoResposta.UsuarioId;
                response.DataCadastro = forumTopicoResposta.DataCadastro;
                response.DataAtualizacao = forumTopicoResposta.DataAtualizacao;
                response.Id = forumTopicoResposta.Id;
                response.ForumTopicoId = forumTopicoResposta.ForumTopicoId;

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
