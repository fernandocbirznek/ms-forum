using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoReplicaFeature.Queries
{
    public class SelecionarForumTopicoReplicaByIdQuery : IRequest<SelecionarForumTopicoReplicaByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarForumTopicoReplicaByIdQueryResponse : Entity
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoRespostaId { get; set; }

        public string UsuarioNome { get; set; }
        public byte[]? UsuarioFoto { get; set; }
    }

    public class SelecionarForumTopicoReplicaByIdQueryHandler : IRequestHandler<SelecionarForumTopicoReplicaByIdQuery, SelecionarForumTopicoReplicaByIdQueryResponse>
    {
        private readonly IRepository<ForumTopicoReplica> _repository;
        private readonly IUsuarioService _usuarioService;

        public SelecionarForumTopicoReplicaByIdQueryHandler
        (
            IRepository<ForumTopicoReplica> repository,
            IUsuarioService usuarioService
        )
        {
            _repository = repository;
            _usuarioService = usuarioService;
        }

        public async Task<SelecionarForumTopicoReplicaByIdQueryResponse> Handle
        (
            SelecionarForumTopicoReplicaByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoReplicaByIdQuery>());

            ForumTopicoReplica forumTopicoReplica = await GetFirstAsync(request, cancellationToken);

            Validator(forumTopicoReplica);

            SelecionarForumTopicoReplicaByIdQueryResponse response = new SelecionarForumTopicoReplicaByIdQueryResponse();

            response.Descricao = forumTopicoReplica.Descricao;
            response.UsuarioId = forumTopicoReplica.UsuarioId;
            response.ForumTopicoRespostaId = forumTopicoReplica.ForumTopicoRespostaId;
            response.DataCadastro = forumTopicoReplica.DataCadastro;
            response.DataAtualizacao = forumTopicoReplica.DataAtualizacao;
            response.Id = forumTopicoReplica.Id;

            var usuario = await _usuarioService.GetUsuarioByIdAsync(forumTopicoReplica.UsuarioId);
            if (usuario is null)
            {
                usuario = new Service.UsuarioService.UsuarioResponse();
            }

            response.UsuarioNome = usuario.Nome;
            response.UsuarioFoto = usuario.Foto;


            return response;
        }

        private void Validator
        (
            ForumTopicoReplica forumTopicoReplica
        )
        {
            if (forumTopicoReplica is null) throw new ArgumentNullException("Fórum tópico replica não encontrado");
        }

        private async Task<ForumTopicoReplica> GetFirstAsync
        (
            SelecionarForumTopicoReplicaByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
