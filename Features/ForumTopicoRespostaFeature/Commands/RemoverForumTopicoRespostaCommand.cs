using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoRespostaFeature.Commands
{
    public class RemoverForumTopicoRespostaCommand : IRequest<RemoverForumTopicoRespostaCommandResponse>
    {
        public long Id { get; set; }
    }

    public class RemoverForumTopicoRespostaCommandResponse
    {
        public long Id { get; set; }
    }

    public class RemoverForumTopicoRespostaCommandHandler : IRequestHandler<RemoverForumTopicoRespostaCommand, RemoverForumTopicoRespostaCommandResponse>
    {
        private readonly IRepository<ForumTopicoResposta> _repository;

        public RemoverForumTopicoRespostaCommandHandler
        (
            IRepository<ForumTopicoResposta> repository
        )
        {
            _repository = repository;
        }

        public async Task<RemoverForumTopicoRespostaCommandResponse> Handle
        (
            RemoverForumTopicoRespostaCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<RemoverForumTopicoRespostaCommand>());

            ForumTopicoResposta forum = await _repository.GetFirstAsync(item => item.Id.Equals(request.Id), cancellationToken);

            Validator(forum);

            await _repository.RemoveAsync(forum);
            await _repository.SaveChangesAsync(cancellationToken);

            RemoverForumTopicoRespostaCommandResponse response = new RemoverForumTopicoRespostaCommandResponse();
            response.Id = forum.Id;

            return response;
        }

        private static void Validator
        (
            ForumTopicoResposta forum
        )
        {
            if (forum is null) throw new ArgumentNullException("Fórum tópico resposta não encontrado");
        }
    }
}
