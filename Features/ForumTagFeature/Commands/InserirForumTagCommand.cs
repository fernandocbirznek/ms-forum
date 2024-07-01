using MediatR;
using ms_forum.Domains;
using ms_forum.Extensions;
using ms_forum.Features.ForumFeature.Commands;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTagFeature.Commands
{
    public class InserirForumTagCommand : IRequest<InserirForumTagCommandResponse>
    {
        public string Titulo { get; set; }
    }

    public class InserirForumTagCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }

        public string Titulo { get; set; }
    }

    public class InserirForumTagHandler : IRequestHandler<InserirForumTagCommand, InserirForumTagCommandResponse>
    {
        private readonly IRepository<ForumTag> _repositoryForum;

        public InserirForumTagHandler
        (
            IRepository<ForumTag> repositoryForum
        )
        {
            _repositoryForum = repositoryForum;
        }

        public async Task<InserirForumTagCommandResponse> Handle
        (
            InserirForumTagCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTagCommand>());

            await Validator(request, cancellationToken);

            ForumTag forum = request.ToDomain();

            await _repositoryForum.AddAsync(forum, cancellationToken);
            await _repositoryForum.SaveChangesAsync(cancellationToken);

            InserirForumTagCommandResponse response = new InserirForumTagCommandResponse();
            response.DataCadastro = forum.DataCadastro;
            response.Id = forum.Id;

            response.Titulo = forum.Titulo;

            return response;
        }

        private async Task Validator
        (
            InserirForumTagCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Titulo)) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumCommand>(item => item.Titulo));
            if (await ExistsTituloAsync(request, cancellationToken)) throw new ArgumentNullException("Título já cadastrado");
        }

        private async Task<bool> ExistsTituloAsync
        (
            InserirForumTagCommand request,
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
