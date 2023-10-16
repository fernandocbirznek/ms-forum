using FluentValidation;
using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoTagFeature.Queries
{
    public class SelecionarForumTopicoTagByIdQuery : IRequest<SelecionarForumTopicoTagByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarForumTopicoTagByIdQueryResponse : Entity
    {
        public string TagTitulo { get; set; }
        public string ForumTopicoTitulo { get; set; }
    }

    public class SelecionarForumTopicoTagByIdQueryHandler : IRequestHandler<SelecionarForumTopicoTagByIdQuery, SelecionarForumTopicoTagByIdQueryResponse>
    {
        private readonly IRepository<ForumTopicoTag> _repository;

        public SelecionarForumTopicoTagByIdQueryHandler
        (
            IRepository<ForumTopicoTag> repository
        )
        {
            _repository = repository;
        }

        public async Task<SelecionarForumTopicoTagByIdQueryResponse> Handle
        (
            SelecionarForumTopicoTagByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoTagByIdQuery>());

            ForumTopicoTag forumTopicoTag = await GetFirstAsync(request, cancellationToken);

            Validator(forumTopicoTag);

            SelecionarForumTopicoTagByIdQueryResponse response = new SelecionarForumTopicoTagByIdQueryResponse();

            response.TagTitulo = forumTopicoTag.ForumTag.Titulo;
            response.ForumTopicoTitulo = forumTopicoTag.ForumTopico.Titulo;
            response.DataCadastro = forumTopicoTag.DataCadastro;
            response.DataAtualizacao = forumTopicoTag.DataAtualizacao;
            response.Id = forumTopicoTag.Id;

            return response;
        }

        private void Validator
        (
            ForumTopicoTag forumTopicoTag
        )
        {
            if (forumTopicoTag is null) throw new ArgumentNullException("Fórum tópico tag não encontrado");
        }

        private async Task<ForumTopicoTag> GetFirstAsync
        (
            SelecionarForumTopicoTagByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken,
                    item => item.ForumTag,
                    item => item.ForumTopico
                );
        }
    }
}
