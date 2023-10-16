using MediatR;
using ms_forum.Domains;
using ms_forum.Extensions;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoTagFeature.Commands
{
    public class AtualizarForumTopicoTagCommand : IRequest<AtualizarForumTopicoTagCommandResponse>
    {
        public long Id { get; set; }
        public long ForumTagId { get; set; }
        public long ForumTopicoId { get; set; }
    }

    public class AtualizarForumTopicoTagCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarForumTopicoTagHandler : IRequestHandler<AtualizarForumTopicoTagCommand, AtualizarForumTopicoTagCommandResponse>
    {
        private readonly IRepository<ForumTopicoTag> _repositoryForumTopicoTag;
        private readonly IRepository<ForumTag> _repositoryForumTag;
        private readonly IRepository<ForumTopico> _repositoryForumTopico;

        public AtualizarForumTopicoTagHandler
        (
            IRepository<ForumTopicoTag> repositoryForumTopicoTag,
            IRepository<ForumTag> repositoryForumTag,
            IRepository<ForumTopico> repositoryForumTopico
        )
        {
            _repositoryForumTopicoTag = repositoryForumTopicoTag;
            _repositoryForumTag = repositoryForumTag;
            _repositoryForumTopico = repositoryForumTopico;
        }

        public async Task<AtualizarForumTopicoTagCommandResponse> Handle
        (
            AtualizarForumTopicoTagCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoTagCommand>());

            Validator(request, cancellationToken);

            await ExistsAsync(request, cancellationToken);

            ForumTopicoTag forum = await GetFirstAsync(request, cancellationToken);
            ForumTopicoTag forumAtualizado = forum.ToUpdate();

            await _repositoryForumTopicoTag.UpdateAsync(forumAtualizado);
            await _repositoryForumTopicoTag.SaveChangesAsync(cancellationToken);

            AtualizarForumTopicoTagCommandResponse response = new AtualizarForumTopicoTagCommandResponse();
            response.DataAtualizacao = forumAtualizado.DataAtualizacao;

            return response;
        }

        private async void Validator
        (
            AtualizarForumTopicoTagCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoTagCommand>(item => item.Id));
            if (request.ForumTagId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoTagCommand>(item => item.ForumTagId));
            if (request.ForumTopicoId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoTagCommand>(item => item.ForumTopicoId));
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Forum tópico topico tag não encontrado");
            if (!(await ExistsForumTagAsync(request, cancellationToken))) throw new ArgumentNullException("Forum tópico tag não encontrado");
            if (!(await ExistsForumTopicoAsync(request, cancellationToken))) throw new ArgumentNullException("Forum tópico topico não encontrado");
        }

        private async Task<bool> ExistsAsync
        (
            AtualizarForumTopicoTagCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTopicoTag.ExistsAsync
                (
                    item => item.Id.Equals(request.Id) &&
                    item.ForumTopicoId.Equals(request.ForumTopicoId) &&
                    item.ForumTagId.Equals(request.ForumTagId),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsForumTagAsync
        (
            AtualizarForumTopicoTagCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTag.ExistsAsync
                (
                    item => item.Id.Equals(request.ForumTagId),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsForumTopicoAsync
        (
            AtualizarForumTopicoTagCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTopico.ExistsAsync
                (
                    item => item.Id.Equals(request.ForumTopicoId),
                    cancellationToken
                );
        }

        private async Task<ForumTopicoTag> GetFirstAsync
        (
            AtualizarForumTopicoTagCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTopicoTag.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
