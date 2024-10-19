using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoReplicaFeature.Queries
{
    public class SelecionarForumTopicoReplicaByForumTopicoIdFiltersQuery :
        IRequest<IEnumerable<SelecionarForumTopicoReplicaByForumTopicoIdFiltersQueryResponse>>
    {
        public long Id { get; set; }
    }

    public class SelecionarForumTopicoReplicaByForumTopicoIdFiltersQueryResponse : Entity
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoId { get; set; }
        public long ForumTopicoRespostaId { get; set; }

        public string UsuarioNome { get; set; }
        public byte[]? UsuarioFoto { get; set; }
    }

    public class SelecionarForumTopicoReplicaByForumTopicoIdFiltersQueryResponseHandler
        : IRequestHandler<SelecionarForumTopicoReplicaByForumTopicoIdFiltersQuery,
            IEnumerable<SelecionarForumTopicoReplicaByForumTopicoIdFiltersQueryResponse>>
    {
        private readonly IRepository<ForumTopicoReplica> _repository;
        private readonly IUsuarioService _usuarioService;

        public SelecionarForumTopicoReplicaByForumTopicoIdFiltersQueryResponseHandler
        (
            IRepository<ForumTopicoReplica> repository,
            IUsuarioService usuarioService
        )
        {
            _repository = repository;
            _usuarioService = usuarioService;
        }

        public async Task<IEnumerable<SelecionarForumTopicoReplicaByForumTopicoIdFiltersQueryResponse>> Handle
        (
            SelecionarForumTopicoReplicaByForumTopicoIdFiltersQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoReplicaByForumTopicoIdFiltersQuery>());

            IEnumerable<ForumTopicoReplica> forumRespostaMany = await _repository.GetAsync
                (
                    item => item.ForumTopicoId.Equals(request.Id),
                    cancellationToken
                );

            List<SelecionarForumTopicoReplicaByForumTopicoIdFiltersQueryResponse> responseMany = new List<SelecionarForumTopicoReplicaByForumTopicoIdFiltersQueryResponse>();

            foreach (ForumTopicoReplica forumTopicoResposta in forumRespostaMany)
            {
                SelecionarForumTopicoReplicaByForumTopicoIdFiltersQueryResponse response = new SelecionarForumTopicoReplicaByForumTopicoIdFiltersQueryResponse();

                response.Descricao = forumTopicoResposta.Descricao;
                response.ForumTopicoRespostaId = forumTopicoResposta.ForumTopicoRespostaId;
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
