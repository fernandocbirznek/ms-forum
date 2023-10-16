using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoTagFeature.Commands
{
    public class RemoverForumTopicoTagCommand : IRequest<RemoverForumTopicoTagCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverForumTopicoTagCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverForumTopicoTagCommandHandler : IRequestHandler<RemoverForumTopicoTagCommand, RemoverForumTopicoTagCommandResponse>
    {
        private readonly IRepository<ForumTopicoTag> _repository;

        public RemoverForumTopicoTagCommandHandler
        (
            IRepository<ForumTopicoTag> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverForumTopicoTagCommandResponse> Handle
        (
            RemoverForumTopicoTagCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverForumTopicoTagCommand>());

            ForumTopicoTag forumTopicoTag = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            Validator(forumTopicoTag);

            await _repository.RemoveAsync(forumTopicoTag);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverForumTopicoTagCommandResponse response = new RemoverForumTopicoTagCommandResponse();
            response.Id = forumTopicoTag.Id;

            return response;
        }

        private void Validator
        (
            ForumTopicoTag forumTopicoTag
        )
        {
            if (forumTopicoTag is null) throw new ArgumentNullException("Fórum tópico tag não encontrado");
        }
    }
}
