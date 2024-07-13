using MediatR;
using ms_forum.Domains;
using ms_forum.Extensions;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoRespostaFeature.Commands
{
    public class InserirForumTopicoRespostaCommand : IRequest<InserirForumTopicoRespostaCommandResponse>
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoId { get; set; }
    }

    public class InserirForumTopicoRespostaCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }

        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoId { get; set; }
    }

    public class InserirForumTopicoRespostaHandler : IRequestHandler<InserirForumTopicoRespostaCommand, InserirForumTopicoRespostaCommandResponse>
    {
        private readonly IRepository<ForumTopicoResposta> _repositoryForum;
        private readonly IRepository<ForumTopico> _repositoryForumTopico;

        public InserirForumTopicoRespostaHandler
        (
            IRepository<ForumTopicoResposta> repositoryForum,
            IRepository<ForumTopico> repositoryForumTopico
        )
        {
            _repositoryForum = repositoryForum;
            _repositoryForumTopico = repositoryForumTopico;
        }

        public async Task<InserirForumTopicoRespostaCommandResponse> Handle
        (
            InserirForumTopicoRespostaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoRespostaCommand>());

            await Validator(request, cancellationToken);

            ForumTopicoResposta forum = request.ToDomain();

            await _repositoryForum.AddAsync(forum, cancellationToken);
            await _repositoryForum.SaveChangesAsync(cancellationToken);

            InserirForumTopicoRespostaCommandResponse response = new InserirForumTopicoRespostaCommandResponse();
            response.DataCadastro = forum.DataCadastro;
            response.Id = forum.Id;

            response.Descricao = forum.Descricao;
            response.ForumTopicoId = forum.ForumTopicoId;
            response.UsuarioId = forum.UsuarioId;

            return response;
        }

        private async Task Validator
        (
            InserirForumTopicoRespostaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (String.IsNullOrEmpty(request.Descricao)) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoRespostaCommand>(item => item.Descricao));
            if (request.ForumTopicoId <= 0) throw new ArgumentNullException(MessageHelper.NullFor<InserirForumTopicoRespostaCommand>(item => item.ForumTopicoId));
            if (!(await ExistsForumTopicoAsync(request, cancellationToken))) throw new ArgumentNullException("Forum tópico resposta não encontrado");
        }

        private async Task<bool> ExistsForumTopicoAsync
            (
                InserirForumTopicoRespostaCommand request,
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
