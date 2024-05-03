using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoReplicaFeature.Commands
{
    public class RemoverForumTopicoReplicaCommand : IRequest<long>
    {
        public long Id { get; set; }
    }

    public class RemoverForumTopicoReplicaCommandHandler : IRequestHandler<RemoverForumTopicoReplicaCommand, long>
    {
        private readonly IRepository<ForumTopicoReplica> _repository;

        public RemoverForumTopicoReplicaCommandHandler
        (
            IRepository<ForumTopicoReplica> repository
        )
        {
            _repository = repository;
        }

        public async Task<long> Handle
        (
            RemoverForumTopicoReplicaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverForumTopicoReplicaCommand>());

            ForumTopicoReplica forumReplica = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            Validator(forumReplica);

            await _repository.RemoveAsync(forumReplica);
            await _repository.SaveChangesAsync(cancellationToken);

            return forumReplica.Id;
        }

        private void Validator
        (
            ForumTopicoReplica forumReplica
        )
        {
            if (forumReplica is null) throw new ArgumentNullException("Fórum tópico replica não encontrado");
        }
    }
}
