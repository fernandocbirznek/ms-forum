using MediatR;
using ms_forum.Domains;
using ms_forum.Extensions;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoFeature.Commands
{
    public class AtualizarForumTopicoCommand : IRequest<AtualizarForumTopicoCommandResponse>
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public IEnumerable<ForumTag> Tags { get; set; }
        public long ForumId { get; set; }
    }

    public class AtualizarForumTopicoCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarForumTopicoHandler : IRequestHandler<AtualizarForumTopicoCommand, AtualizarForumTopicoCommandResponse>
    {
        private readonly IRepository<ForumTopico> _repositoryForumTopico;
        private readonly IRepository<Forum> _repositoryForum;

        public AtualizarForumTopicoHandler
        (
            IRepository<ForumTopico> repositoryForumTopico,
            IRepository<Forum> repositoryForum
        )
        {
            _repositoryForumTopico = repositoryForumTopico;
            _repositoryForum = repositoryForum;
        }

        public async Task<AtualizarForumTopicoCommandResponse> Handle(AtualizarForumTopicoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoCommand>());

            await Validator(request, cancellationToken);

            ForumTopico forum = await GetFirstAsync(request, cancellationToken);
            ForumTopico forumAtualizado = forum.ToUpdate();

            await _repositoryForumTopico.UpdateAsync(forumAtualizado);
            await _repositoryForumTopico.SaveChangesAsync(cancellationToken);

            AtualizarForumTopicoCommandResponse response = new AtualizarForumTopicoCommandResponse();
            response.DataAtualizacao = forumAtualizado.DataAtualizacao;

            return response;
        }

        private async Task Validator
        (
            AtualizarForumTopicoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoCommand>(item => item.Id));
            if (String.IsNullOrEmpty(request.Titulo)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoCommand>(item => item.Titulo));
            if (String.IsNullOrEmpty(request.Descricao)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoCommand>(item => item.Descricao));
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Forum Topico não encontrado");
            if (!(await ExistsForumAsync(request, cancellationToken))) throw new ArgumentNullException("Forum não encontrado");
        }

        private async Task<ForumTopico> GetFirstAsync
        (
            AtualizarForumTopicoCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTopico.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsAsync
        (
            AtualizarForumTopicoCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTopico.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsForumAsync
        (
            AtualizarForumTopicoCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForum.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
