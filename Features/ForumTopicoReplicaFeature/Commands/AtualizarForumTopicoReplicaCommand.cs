using MediatR;
using ms_forum.Domains;
using ms_forum.Extensions;
using ms_forum.Features.ForumTopicoFeature.Commands;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoReplicaFeature.Commands
{
    public class AtualizarForumTopicoReplicaCommand : IRequest<AtualizarForumTopicoReplicaCommandResponse>
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
    }

    public class AtualizarForumTopicoReplicaCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarForumTopicoReplicaHandler : IRequestHandler<AtualizarForumTopicoReplicaCommand, AtualizarForumTopicoReplicaCommandResponse>
    {
        private readonly IRepository<ForumTopicoReplica> _repositoryForumReplica;

        public AtualizarForumTopicoReplicaHandler
        (
            IRepository<ForumTopicoReplica> repositoryForumTopicoReplica
        )
        {
            _repositoryForumReplica = repositoryForumTopicoReplica;
        }

        public async Task<AtualizarForumTopicoReplicaCommandResponse> Handle
        (
            AtualizarForumTopicoReplicaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoReplicaCommand>());

            await Validator(request, cancellationToken);

            ForumTopicoReplica forumTopicoReplica = await GetFirstAsync(request, cancellationToken);
            forumTopicoReplica.Descricao = request.Descricao;
            forumTopicoReplica.DataAtualizacao = DateTime.Now;

            await _repositoryForumReplica.UpdateAsync(forumTopicoReplica);
            await _repositoryForumReplica.SaveChangesAsync(cancellationToken);

            AtualizarForumTopicoReplicaCommandResponse response = new AtualizarForumTopicoReplicaCommandResponse();
            response.DataAtualizacao = forumTopicoReplica.DataAtualizacao;

            return response;
        }

        private async Task Validator
        (
            AtualizarForumTopicoReplicaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoReplicaCommand>(item => item.Id));
            if (String.IsNullOrEmpty(request.Descricao)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoCommand>(item => item.Descricao));
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Forum tópico replica não encontrado");
        }

        private async Task<ForumTopicoReplica> GetFirstAsync
        (
            AtualizarForumTopicoReplicaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumReplica.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsAsync
        (
            AtualizarForumTopicoReplicaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumReplica.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
