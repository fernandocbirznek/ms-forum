using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumFeature.Commands
{
    public class RemoverForumCommand : IRequest<long>
    {
        public long Id { get; set; }
    }

    public class RemoverForumCommandHandler : IRequestHandler<RemoverForumCommand, long>
    {
        private readonly IRepository<Forum> _repository;

        public RemoverForumCommandHandler
        (
            IRepository<Forum> repository
        )
        {
            _repository = repository;
        }

        public async Task<long> Handle
        (
            RemoverForumCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverForumCommand>());

            await Validator(request, cancellationToken);

            Forum forum = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await _repository.RemoveAsync(forum);
            await _repository.SaveChangesAsync(cancellationToken);

            return forum.Id;
        }

        private async Task Validator
        (
            RemoverForumCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Fórum não encontrado");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverForumCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
