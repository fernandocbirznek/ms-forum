using MediatR;
using ms_forum.Domains;
using ms_forum.Extensions;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumFeature.Commands
{
    public class AtualizarForumCommand : IRequest<AtualizarForumCommandResponse>
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }

    public class AtualizarForumCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadasatro { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }

    public class AtualizarForumHandler : IRequestHandler<AtualizarForumCommand, AtualizarForumCommandResponse>
    {
        private readonly IRepository<Forum> _repositoryForum;

        public AtualizarForumHandler
        (
            IRepository<Forum> repositoryForum
        )
        {
            _repositoryForum = repositoryForum;
        }

        public async Task<AtualizarForumCommandResponse> Handle(AtualizarForumCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumCommand>());

            await Validator(request, cancellationToken);

            Forum forum = await GetFirstAsync(request, cancellationToken);
            forum.Titulo = request.Titulo;
            forum.Descricao = request.Descricao;

            await _repositoryForum.UpdateAsync(forum);
            await _repositoryForum.SaveChangesAsync(cancellationToken);

            AtualizarForumCommandResponse response = new AtualizarForumCommandResponse();
            response.Id = forum.Id;
            response.DataCadasatro = forum.DataCadastro;
            response.DataAtualizacao = forum.DataAtualizacao;

            response.Titulo = forum.Titulo;
            response.Descricao = forum.Descricao;

            return response;
        }

        private async Task Validator
        (
            AtualizarForumCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumCommand>(item => item.Id));
            if (String.IsNullOrEmpty(request.Titulo)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumCommand>(item => item.Titulo));
            if (String.IsNullOrEmpty(request.Descricao)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumCommand>(item => item.Descricao));
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Fórum não encontrado");
            if (await ExistsTituloAsync(request, cancellationToken)) throw new ArgumentNullException("Título já cadastrado");
        }

        private async Task<Forum> GetFirstAsync
        (
            AtualizarForumCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForum.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsAsync
        (
            AtualizarForumCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForum.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsTituloAsync
        (
            AtualizarForumCommand request,
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
