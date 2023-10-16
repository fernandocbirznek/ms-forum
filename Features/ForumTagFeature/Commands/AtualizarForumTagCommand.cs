using MediatR;
using ms_forum.Domains;
using ms_forum.Extensions;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTagFeature.Commands
{
    public class AtualizarForumTagCommand : IRequest<AtualizarForumTagCommandResponse>
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
    }

    public class AtualizarForumTagCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarForumTagHandler : IRequestHandler<AtualizarForumTagCommand, AtualizarForumTagCommandResponse>
    {
        private readonly IRepository<ForumTag> _repositoryForum;

        public AtualizarForumTagHandler
        (
            IRepository<ForumTag> repositoryForum
        )
        {
            _repositoryForum = repositoryForum;
        }

        public async Task<AtualizarForumTagCommandResponse> Handle(AtualizarForumTagCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTagCommand>());

            await Validator(request, cancellationToken);

            ForumTag forumTag = await GetFirstAsync(request, cancellationToken);
            forumTag.Titulo = request.Titulo;
            forumTag.DataAtualizacao = DateTime.Now;

            await _repositoryForum.UpdateAsync(forumTag);
            await _repositoryForum.SaveChangesAsync(cancellationToken);

            AtualizarForumTagCommandResponse response = new AtualizarForumTagCommandResponse();
            response.DataAtualizacao = forumTag.DataAtualizacao;

            return response;
        }

        private async Task Validator
        (
            AtualizarForumTagCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTagCommand>(item => item.Id));
            if (String.IsNullOrEmpty(request.Titulo)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTagCommand>(item => item.Titulo));
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Fórum não encontrado");
            if (await ExistsTituloAsync(request, cancellationToken)) throw new ArgumentNullException("Título já cadastrado");
        }

        private async Task<bool> ExistsTituloAsync
        (
            AtualizarForumTagCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForum.ExistsAsync
                (
                    item => item.Titulo.ToLower().Trim().Equals(request.Titulo.ToLower().Trim()),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsAsync
        (
            AtualizarForumTagCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForum.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<ForumTag> GetFirstAsync
        (
            AtualizarForumTagCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForum.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
