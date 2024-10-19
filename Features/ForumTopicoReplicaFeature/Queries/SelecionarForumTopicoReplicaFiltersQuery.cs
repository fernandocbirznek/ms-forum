using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;
using ms_forum.Service;

namespace ms_forum.Features.ForumTopicoReplicaFeature.Queries
{
    public class SelecionarForumTopicoReplicaFiltersQuery : IRequest<IEnumerable<SelecionarForumTopicoReplicaFiltersQueryResponse>>
    {
    }

    public class SelecionarForumTopicoReplicaFiltersQueryResponse : Entity
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoId { get; set; }
        public long ForumTopicoRespostaId { get; set; }

        public string UsuarioNome { get; set; }
        public byte[]? UsuarioFoto { get; set; }
    }

    public class SelecionarForumTopicoReplicaFiltersQueryResponseHandler : IRequestHandler<SelecionarForumTopicoReplicaFiltersQuery, IEnumerable<SelecionarForumTopicoReplicaFiltersQueryResponse>>
    {
        private readonly IRepository<ForumTopicoReplica> _repository;
        private readonly IUsuarioService _usuarioService;

        public SelecionarForumTopicoReplicaFiltersQueryResponseHandler
        (
            IRepository<ForumTopicoReplica> repository,
            IUsuarioService usuarioService
        )
        {
            _repository = repository;
            _usuarioService = usuarioService;
        }

        public async Task<IEnumerable<SelecionarForumTopicoReplicaFiltersQueryResponse>> Handle
        (
            SelecionarForumTopicoReplicaFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoReplicaFiltersQuery>());

            IEnumerable<ForumTopicoReplica> forumRespostaMany = await _repository.GetAsync(cancellationToken);

            List<SelecionarForumTopicoReplicaFiltersQueryResponse> responseMany = new List<SelecionarForumTopicoReplicaFiltersQueryResponse>();

            foreach (ForumTopicoReplica forumTopicoResposta in forumRespostaMany)
            {
                SelecionarForumTopicoReplicaFiltersQueryResponse response = new SelecionarForumTopicoReplicaFiltersQueryResponse();

                response.Descricao = forumTopicoResposta.Descricao;
                response.ForumTopicoRespostaId = forumTopicoResposta.ForumTopicoRespostaId;
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
