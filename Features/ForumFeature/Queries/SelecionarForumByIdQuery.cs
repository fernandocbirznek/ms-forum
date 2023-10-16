using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumFeature.Queries
{
    public class SelecionarForumByIdQuery : IRequest<SelecionarForumByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarForumByIdQueryResponse : Entity
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
    }

    public class SelecionarForumByIdQueryHandler : IRequestHandler<SelecionarForumByIdQuery, SelecionarForumByIdQueryResponse>
    {
        private readonly IRepository<Forum> _repository;

        public SelecionarForumByIdQueryHandler
        (
            IRepository<Forum> repository
        )
        {
            _repository = repository;
        }

        public async Task<SelecionarForumByIdQueryResponse> Handle
        (
            SelecionarForumByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumByIdQuery>());

            Forum forum = await GetFirstAsync(request, cancellationToken);

            Validator(forum, cancellationToken);

            SelecionarForumByIdQueryResponse response = new SelecionarForumByIdQueryResponse();

            response.Titulo = forum.Titulo;
            response.Descricao = forum.Descricao;
            response.DataCadastro = forum.DataCadastro;
            response.DataAtualizacao = forum.DataAtualizacao;
            response.Id = forum.Id;

            return response;
        }

        private async void Validator
        (
            Forum forum,
            CancellationToken cancellationToken
        )
        {
            if (forum is null) throw new ArgumentNullException("Fórum não encontrado");
        }

        private async Task<Forum> GetFirstAsync
        (
            SelecionarForumByIdQuery request,
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
