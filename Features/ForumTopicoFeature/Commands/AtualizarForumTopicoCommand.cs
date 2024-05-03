using MediatR;
using ms_forum.Domains;
using ms_forum.Enum;
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
        public ForumTopicoEnum ForumTopicoEnum { get; set; }
        public IEnumerable<ForumTag> ForumTagMany { get; set; }
        public long ForumId { get; set; }
    }

    public class AtualizarForumTopicoCommandResponse
    {
        public DateTime DataAtualizacao { get; set; }
    }

    public class AtualizarForumTopicoHandler : IRequestHandler<AtualizarForumTopicoCommand, AtualizarForumTopicoCommandResponse>
    {
        private readonly IRepository<ForumTopico> _repositoryForumTopico;
        private readonly IRepository<ForumTopicoTag> _repositoryForumTopicoTag;
        private readonly IRepository<Forum> _repositoryForum;

        public AtualizarForumTopicoHandler
        (
            IRepository<ForumTopico> repositoryForumTopico,
            IRepository<ForumTopicoTag> repositoryForumTopicoTag,
            IRepository<Forum> repositoryForum
        )
        {
            _repositoryForumTopico = repositoryForumTopico;
            _repositoryForumTopicoTag = repositoryForumTopicoTag;
            _repositoryForum = repositoryForum;
        }

        public async Task<AtualizarForumTopicoCommandResponse> Handle(AtualizarForumTopicoCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoCommand>());

            await Validator(request, cancellationToken);

            ForumTopico forum = await GetFirstAsync(request, cancellationToken);
            forum.Titulo = request.Titulo;
            forum.Descricao = request.Descricao;
            forum.ForumTopicoEnum = request.ForumTopicoEnum;
            forum.DataAtualizacao = DateTime.Now;

            await _repositoryForumTopico.UpdateAsync(forum);
            await _repositoryForumTopico.SaveChangesAsync(cancellationToken);

            IEnumerable<ForumTopicoTag> forumTopicoMany = await GetForumTopicoTagAsync(request, cancellationToken);

            foreach (ForumTag tag in request.ForumTagMany)
            {
                ForumTopicoTag inserirForumTopicoTag = new ForumTopicoTag();
                inserirForumTopicoTag.ForumTagId = tag.Id;
                inserirForumTopicoTag.ForumTopicoId = request.Id;

                if (!forumTopicoMany.Any(item => item.ForumTagId.Equals(inserirForumTopicoTag.ForumTagId)))
                {
                    await _repositoryForumTopicoTag.UpdateAsync(inserirForumTopicoTag);
                    await _repositoryForumTopicoTag.SaveChangesAsync(cancellationToken);
                }
            }

            foreach (ForumTopicoTag forumTopicoTag in forumTopicoMany)
            {
                if (!request.ForumTagMany.Any(item => item.Equals(forumTopicoTag.ForumTagId)))
                {
                    await _repositoryForumTopicoTag.RemoveAsync(forumTopicoTag);
                    await _repositoryForumTopicoTag.SaveChangesAsync(cancellationToken);
                }
            }

            AtualizarForumTopicoCommandResponse response = new AtualizarForumTopicoCommandResponse();
            response.DataAtualizacao = forum.DataAtualizacao;

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

        private async Task<IEnumerable<ForumTopicoTag>> GetForumTopicoTagAsync
        (
            AtualizarForumTopicoCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTopicoTag.GetAsync
                (
                    item => item.ForumTopicoId.Equals(request.Id),
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
