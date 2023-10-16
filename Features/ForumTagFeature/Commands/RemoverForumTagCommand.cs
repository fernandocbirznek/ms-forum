using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTagFeature.Commands
{
    public class RemoverForumTagCommand : IRequest<RemoverForumTagCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverForumTagCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverForumTagCommandHandler : IRequestHandler<RemoverForumTagCommand, RemoverForumTagCommandResponse>
    {
        private readonly IRepository<ForumTag> _repository;

        public RemoverForumTagCommandHandler
        (
            IRepository<ForumTag> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverForumTagCommandResponse> Handle
        (
            RemoverForumTagCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverForumTagCommand>());

            await Validator(request, cancellationToken);

            ForumTag forum = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await _repository.RemoveAsync(forum);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverForumTagCommandResponse response = new RemoverForumTagCommandResponse();
            response.Id = forum.Id;

            return response;
        }

        private async Task Validator
        (
            RemoverForumTagCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Fórum não encontrado");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverForumTagCommand request,
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
