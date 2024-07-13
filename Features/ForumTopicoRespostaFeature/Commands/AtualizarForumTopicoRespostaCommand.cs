using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoRespostaFeature.Commands
{
    public class AtualizarForumTopicoRespostaCommand : IRequest<AtualizarForumTopicoRespostaCommandResponse>
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
    }

    public class AtualizarForumTopicoRespostaCommandResponse
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public long ForumTopicoId { get; set; }
    }

    public class AtualizarForumTopicoRespostaHandler : IRequestHandler<AtualizarForumTopicoRespostaCommand, AtualizarForumTopicoRespostaCommandResponse>
    {
        private readonly IRepository<ForumTopicoResposta> _repositoryForumTopicoResposta;

        public AtualizarForumTopicoRespostaHandler
        (
            IRepository<ForumTopicoResposta> repositoryForumTopicoResposta
        )
        {
            _repositoryForumTopicoResposta = repositoryForumTopicoResposta;
        }

        public async Task<AtualizarForumTopicoRespostaCommandResponse> Handle
        (
            AtualizarForumTopicoRespostaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoRespostaCommand>());

            await Validator(request, cancellationToken);

            ForumTopicoResposta forumTopicoResposta = await GetFirstAsync(request, cancellationToken);
            forumTopicoResposta.Descricao = request.Descricao;
            forumTopicoResposta.DataAtualizacao = DateTime.Now;

            await _repositoryForumTopicoResposta.UpdateAsync(forumTopicoResposta);
            await _repositoryForumTopicoResposta.SaveChangesAsync(cancellationToken);

            AtualizarForumTopicoRespostaCommandResponse response = new AtualizarForumTopicoRespostaCommandResponse();
            response.DataAtualizacao = forumTopicoResposta.DataAtualizacao;
            response.Id = forumTopicoResposta.Id;
            response.DataCadastro = forumTopicoResposta.DataCadastro;

            response.Descricao = forumTopicoResposta.Descricao;
            response.ForumTopicoId = forumTopicoResposta.ForumTopicoId;
            response.UsuarioId = forumTopicoResposta.UsuarioId;

            return response;
        }

        private async Task Validator
        (
            AtualizarForumTopicoRespostaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.Id <= 0) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoRespostaCommand>(item => item.Id));
            if (String.IsNullOrEmpty(request.Descricao)) throw new ArgumentNullException(MessageHelper.NullFor<AtualizarForumTopicoRespostaCommand>(item => item.Descricao));
            if (!(await ExistsAsync(request, cancellationToken))) throw new ArgumentNullException("Forum tópico resposta não encontrado");
        }

        private async Task<ForumTopicoResposta> GetFirstAsync
        (
            AtualizarForumTopicoRespostaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTopicoResposta.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }

        private async Task<bool> ExistsAsync
        (
            AtualizarForumTopicoRespostaCommand request,
            CancellationToken cancellationToken
        )
        {
            return await _repositoryForumTopicoResposta.ExistsAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
