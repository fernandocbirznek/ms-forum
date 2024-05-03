using MediatR;
using ms_forum.Domains;
using ms_forum.Enum;
using ms_forum.Extensions;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoFeature.Commands
{
    public class InserirForumTopicoCommand : IRequest<InserirForumTopicoCommandResponse>
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumId { get; set; }
        public ForumTopicoEnum ForumTopicoEnum { get; set; }
        public IEnumerable<ForumTag> ForumTagMany { get; set; }
    }

    public class InserirForumTopicoCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class InserirForumTopicoHandler : IRequestHandler<InserirForumTopicoCommand, InserirForumTopicoCommandResponse>
    {
        private readonly IRepository<ForumTopico> _repositoryForumTopico;
        private readonly IRepository<Forum> _repositoryForum;
        private readonly IRepository<ForumTopicoTag> _repositoryForumTopicoTag;

        public InserirForumTopicoHandler
        (
            IRepository<ForumTopico> repositoryForum,
            IRepository<Forum> repository,
            IRepository<ForumTopicoTag> repositoryForumTopicoTag
        )
        {
            _repositoryForumTopico = repositoryForum;
            _repositoryForum = repository;
            _repositoryForumTopicoTag = repositoryForumTopicoTag;
        }

        public async Task<InserirForumTopicoCommandResponse> Handle
        (
            InserirForumTopicoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoCommand>());

            await Validator(request, cancellationToken);

            ForumTopico forumTopico = request.ToDomain();

            await _repositoryForumTopico.AddAsync(forumTopico, cancellationToken);
            await _repositoryForumTopico.SaveChangesAsync(cancellationToken);

            if (request.ForumTagMany.Count() > 0)
                foreach (ForumTag tag in request.ForumTagMany)
                {
                    ForumTopicoTag inserirForumTopicoTag = new()
                    {
                        ForumTopicoId = forumTopico.Id,
                        ForumTagId = tag.Id,
                        DataCadastro = DateTime.Now
                    }; ;

                    await _repositoryForumTopicoTag.AddAsync(inserirForumTopicoTag, cancellationToken);
                    await _repositoryForumTopicoTag.SaveChangesAsync(cancellationToken);
                }

            InserirForumTopicoCommandResponse response = new InserirForumTopicoCommandResponse();
            response.DataCadastro = forumTopico.DataCadastro;
            response.Id = forumTopico.Id;

            return response;
        }

        private async Task Validator
        (
            InserirForumTopicoCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Titulo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoCommand>(item => item.Titulo));
            if (String.IsNullOrEmpty(request.Descricao)) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoCommand>(item => item.Descricao));
            if (request.ForumId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoCommand>(item => item.ForumId));
            if (!(await ExistsForumAsync(request, cancellationToken))) throw new ArgumentNullException("Fórum não encontrado");
        }

        private async Task<bool> ExistsForumAsync
        (
            InserirForumTopicoCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForum.ExistsAsync
                (
                    item => item.Id.Equals(request.ForumId),
                    cancellationToken
                );
        }
    }
}
