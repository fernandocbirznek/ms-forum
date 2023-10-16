using MediatR;
using ms_forum.Domains;
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

        public InserirForumTopicoHandler
        (
            IRepository<ForumTopico> repositoryForum,
            IRepository<Forum> repository
        )
        {
            _repositoryForumTopico = repositoryForum;
            _repositoryForum = repository;
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

            ForumTopico forum = request.ToDomain();

            await _repositoryForumTopico.AddAsync(forum, cancellationToken);
            await _repositoryForumTopico.SaveChangesAsync(cancellationToken);

            InserirForumTopicoCommandResponse response = new InserirForumTopicoCommandResponse();
            response.DataCadastro = forum.DataCadastro;
            response.Id = forum.Id;

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
