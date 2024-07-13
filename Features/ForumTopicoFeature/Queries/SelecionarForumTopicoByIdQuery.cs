using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoFeature.Queries
{
    public class SelecionarForumTopicoByIdQuery : IRequest<SelecionarForumTopicoByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarForumTopicoByIdQueryResponse : Entity
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }
        public IEnumerable<ForumTag> ForumTagMany { get; set; }
        public long ForumId { get; set; }
    }

    public class SelecionarForumTopicoByIdQueryHandler : IRequestHandler<SelecionarForumTopicoByIdQuery, SelecionarForumTopicoByIdQueryResponse>
    {
        private readonly IRepository<ForumTopico> _repository;

        public SelecionarForumTopicoByIdQueryHandler
        (
            IRepository<ForumTopico> repository
        )
        {
            _repository = repository;
        }

        public async Task<SelecionarForumTopicoByIdQueryResponse> Handle
        (
            SelecionarForumTopicoByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoByIdQuery>());

            ForumTopico forumTopico = await GetFirstAsync(request, cancellationToken);

            Validator(forumTopico);

            SelecionarForumTopicoByIdQueryResponse response = new SelecionarForumTopicoByIdQueryResponse();

            response.Titulo = forumTopico.Titulo;
            response.Descricao = forumTopico.Descricao;
            response.UsuarioId = forumTopico.UsuarioId;
            response.ForumId = forumTopico.ForumId;
            response.DataCadastro = forumTopico.DataCadastro;
            response.DataAtualizacao = forumTopico.DataAtualizacao;
            response.Id = forumTopico.Id;

            return response;
        }

        private static void Validator
        (
            ForumTopico forumTopico
        )
        {
            if (forumTopico is null) throw new ArgumentNullException("Fórum tópico não encontrado");
        }

        private async Task<ForumTopico> GetFirstAsync
        (
            SelecionarForumTopicoByIdQuery request,
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
