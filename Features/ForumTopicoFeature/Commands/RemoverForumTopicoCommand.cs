using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoFeature.Commands
{
    public class RemoverForumTopicoCommand : IRequest<RemoverForumTopicoCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverForumTopicoCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverForumTopicoCommandHandler : IRequestHandler<RemoverForumTopicoCommand, RemoverForumTopicoCommandResponse>
    {
        private readonly IRepository<ForumTopico> _repository;

        public RemoverForumTopicoCommandHandler
        (
            IRepository<ForumTopico> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverForumTopicoCommandResponse> Handle
        (
            RemoverForumTopicoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverForumTopicoCommand>());

            ForumTopico forum = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            await Validator(request, cancellationToken);

            await _repository.RemoveAsync(forum);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverForumTopicoCommandResponse response = new RemoverForumTopicoCommandResponse();
            response.Id = forum.Id;

            return response;
        }

        private async Task Validator
        (
            RemoverForumTopicoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Fórum tópico não encontrado");
        }

        private async Task<bool> ExistsAsync
        (
            RemoverForumTopicoCommand request,
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
