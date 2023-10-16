using MediatR;
using ms_forum.Domains;
using ms_forum.Extensions;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoTagFeature.Commands
{
    public class InserirForumTopicoTagCommand : IRequest<InserirForumTopicoTagCommandResponse>
    {
        public long ForumTagId { get; set; }
        public long ForumTopicoId { get; set; }
    }


    public class InserirForumTopicoTagCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirForumTopicoTagHandler : IRequestHandler<InserirForumTopicoTagCommand, InserirForumTopicoTagCommandResponse>
    {
        private readonly IRepository<ForumTopicoTag> _repositoryForumTopicoTag;
        private readonly IRepository<ForumTag> _repositoryForumTag;
        private readonly IRepository<ForumTopico> _repositoryForumTopico;

        public InserirForumTopicoTagHandler
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

        public async Task<InserirForumTopicoTagCommandResponse> Handle
        (
            InserirForumTopicoTagCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoTagCommand>());

            await Validator(request, cancellationToken);

            ForumTopicoTag forumTopicoTag = request.ToDomain();

            await _repositoryForumTopicoTag.AddAsync(forumTopicoTag, cancellationToken);
            await _repositoryForumTopicoTag.SaveChangesAsync(cancellationToken);

            InserirForumTopicoTagCommandResponse response = new InserirForumTopicoTagCommandResponse();
            response.DataCadastro = forumTopicoTag.DataCadastro;
            response.Id = forumTopicoTag.Id;

            return response;
        }

        private async Task Validator
        (
            InserirForumTopicoTagCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.ForumTagId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoTagCommand>(item => item.ForumTagId));
            if (request.ForumTopicoId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoTagCommand>(item => item.ForumTopicoId));
            if (!(await ExistsForumTagAsync(request, cancellationToken))) throw new ArgumentNullException("Forum tópico tag não encontrado");
            if (!(await ExistsForumTopicoAsync(request, cancellationToken))) throw new ArgumentNullException("Forum tópico topico não encontrado");
        }

        private async Task<bool> ExistsForumTagAsync
        (
            InserirForumTopicoTagCommand request,
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
            InserirForumTopicoTagCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTopico.ExistsAsync
                (
                    item => item.Id.Equals(request.ForumTopicoId),
                    cancellationToken
                );
        }
    }
}
