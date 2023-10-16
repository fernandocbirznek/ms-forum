using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTagFeature.Queries
{
    public class SelecionarForumTagByIdQuery : IRequest<SelecionarForumTagByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarForumTagByIdQueryResponse : Entity
    {
        public string Titulo { get; set; }
    }

    public class SelecionarForumTagByIdQueryHandler : IRequestHandler<SelecionarForumTagByIdQuery, SelecionarForumTagByIdQueryResponse>
    {
        private readonly IRepository<ForumTag> _repository;

        public SelecionarForumTagByIdQueryHandler
        (
            IRepository<ForumTag> repository
        )
        {
            _repository = repository;
        }

        public async Task<SelecionarForumTagByIdQueryResponse> Handle
        (
            SelecionarForumTagByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTagByIdQuery>());

            ForumTag forumTag = await GetFirstAsync(request, cancellationToken);

            Validator(forumTag, cancellationToken);

            SelecionarForumTagByIdQueryResponse response = new SelecionarForumTagByIdQueryResponse();

            response.Titulo = forumTag.Titulo;
            response.DataCadastro = forumTag.DataCadastro;
            response.DataAtualizacao = forumTag.DataAtualizacao;
            response.Id = forumTag.Id;

            return response;
        }

        private async void Validator
        (
            ForumTag forumTag,
            CancellationToken cancellationToken
        )
        {
            if (forumTag is null) throw new ArgumentNullException("Fórum tag não encontrado");
        }

        private async Task<ForumTag> GetFirstAsync
        (
            SelecionarForumTagByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken
                );
        }
    }
}
