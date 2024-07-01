using MediatR;
using ms_forum.Domains;
using ms_forum.Extensions;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumFeature.Commands
{
    public class InserirForumCommand : IRequest<InserirForumCommandResponse>
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }

    public class InserirForumCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }

        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }

    public class InserirForumHandler : IRequestHandler<InserirForumCommand, InserirForumCommandResponse>
    {
        private readonly IRepository<Forum> _repositoryForum;

        public InserirForumHandler
        (
            IRepository<Forum> repositoryForum
        )
        {
            _repositoryForum = repositoryForum;
        }

        public async Task<InserirForumCommandResponse> Handle
        (
            InserirForumCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirForumCommand>());

            await Validator(request, cancellationToken);

            Forum forum = request.ToDomain();

            await _repositoryForum.AddAsync(forum, cancellationToken);
            await _repositoryForum.SaveChangesAsync(cancellationToken);

            InserirForumCommandResponse response = new InserirForumCommandResponse();
            response.DataCadastro = forum.DataCadastro;
            response.Id = forum.Id;

            response.Titulo = forum.Titulo;
            response.Descricao = forum.Descricao;

            return response;
        }

        private async Task Validator
        (
            InserirForumCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Titulo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumCommand>(item => item.Titulo));
            if (String.IsNullOrEmpty(request.Descricao)) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumCommand>(item => item.Descricao));
            if (await ExistsTituloAsync(request, cancellationToken)) throw new ArgumentNullException("Título já cadastrado");
        }

        private async Task<bool> ExistsTituloAsync
        (
            InserirForumCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForum.ExistsAsync
                (
                    item => item.Titulo.ToLower().Trim().Equals(request.Titulo.ToLower().Trim()),
                    cancellationToken
                );
        }
    }
}
