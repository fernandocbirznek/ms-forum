using MediatR;
using ms_forum.Domains;
using ms_forum.Extensions;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoReplicaFeature.Commands
{
    public class InserirForumTopicoReplicaCommand : IRequest<InserirForumTopicoReplicaCommandResponse>
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoRespostaId { get; set; }
    }

    public class InserirForumTopicoReplicaCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirForumTopicoReplicaHandler : IRequestHandler<InserirForumTopicoReplicaCommand, InserirForumTopicoReplicaCommandResponse>
    {
        private readonly IRepository<ForumTopicoReplica> _repositoryForumTopicoReplica;
        private readonly IRepository<ForumTopicoResposta> _repositoryForumTopicoResposta;

        public InserirForumTopicoReplicaHandler
        (
            IRepository<ForumTopicoReplica> repositoryForumTopicoReplica,
            IRepository<ForumTopicoResposta> repositoryForumTopicoResposta
        )
        {
            _repositoryForumTopicoReplica = repositoryForumTopicoReplica;
            _repositoryForumTopicoResposta = repositoryForumTopicoResposta;
        }

        public async Task<InserirForumTopicoReplicaCommandResponse> Handle
        (
            InserirForumTopicoReplicaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoReplicaCommand>());

            await Validator(request, cancellationToken);

            ForumTopicoReplica forum = request.ToDomain();

            await _repositoryForumTopicoReplica.AddAsync(forum, cancellationToken);
            await _repositoryForumTopicoReplica.SaveChangesAsync(cancellationToken);

            InserirForumTopicoReplicaCommandResponse response = new InserirForumTopicoReplicaCommandResponse();
            response.DataCadastro = forum.DataCadastro;
            response.Id = forum.Id;

            return response;
        }

        private async Task Validator
        (
            InserirForumTopicoReplicaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Descricao)) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoReplicaCommand>(item => item.Descricao));
            if (request.ForumTopicoRespostaId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoReplicaCommand>(item => item.ForumTopicoRespostaId));
            if (!(await ExistsForumRespostaAsync(request, cancellationToken))) throw new ArgumentNullException("Forum tópico resposta não encontrado");
        }

        private async Task<bool> ExistsForumRespostaAsync
            (
                InserirForumTopicoReplicaCommand request,
                CancellationToken cancellationToken
            )
        {
            return await _repositoryForumTopicoResposta.ExistsAsync
                (
                    item => item.Id.Equals(request.ForumTopicoRespostaId),
                    cancellationToken
                );
        }
    }
}
