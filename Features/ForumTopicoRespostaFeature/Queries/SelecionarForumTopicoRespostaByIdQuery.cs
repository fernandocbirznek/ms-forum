using FluentValidation;
using MediatR;
using ms_forum.Domains;
using ms_forum.Helpers;
using ms_forum.Interface;

namespace ms_forum.Features.ForumTopicoRespostaFeature.Queries
{
    public class SelecionarForumTopicoRespostaByIdQuery : IRequest<SelecionarForumTopicoRespostaByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarForumTopicoRespostaByIdQueryResponse : Entity
    {
        public string Descricao { get; set; }
        public long UsuarioId { get; set; }

        public string UsuarioNome { get; set; }
        public byte[]? UsuarioFoto { get; set; }
    }

    public class SelecionarForumTopicoRespostaByIdQueryHandler : IRequestHandler<SelecionarForumTopicoRespostaByIdQuery, SelecionarForumTopicoRespostaByIdQueryResponse>
    {
        private readonly IRepository<ForumTopicoResposta> _repository;
        private readonly IUsuarioService _usuarioService;

        public SelecionarForumTopicoRespostaByIdQueryHandler
        (
            IRepository<ForumTopicoResposta> repository,
            IUsuarioService usuarioService
        )
        {
            _repository = repository;
            _usuarioService = usuarioService;
        }

        public async Task<SelecionarForumTopicoRespostaByIdQueryResponse> Handle
        (
            SelecionarForumTopicoRespostaByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarForumTopicoRespostaByIdQuery>());

            ForumTopicoResposta forumTopicoResposta = await GetFirstAsync(request, cancellationToken);

            Validator(forumTopicoResposta);

            SelecionarForumTopicoRespostaByIdQueryResponse response = new SelecionarForumTopicoRespostaByIdQueryResponse();

            response.Descricao = forumTopicoResposta.Descricao;
            response.UsuarioId = forumTopicoResposta.UsuarioId;
            response.DataCadastro = forumTopicoResposta.DataCadastro;
            response.DataAtualizacao = forumTopicoResposta.DataAtualizacao;
            response.Id = forumTopicoResposta.Id;

            var usuario = await _usuarioService.GetUsuarioByIdAsync(forumTopicoResposta.UsuarioId);
            if (usuario is null)
            {
                usuario = new Service.UsuarioService.UsuarioResponse();
            }

            response.UsuarioNome = usuario.Nome;
            response.UsuarioFoto = usuario.Foto;

            return response;
        }

        private static void Validator
        (
            ForumTopicoResposta forumTopicoResposta
        )
        {
            if (forumTopicoResposta is null) throw new ArgumentNullException("Fórum tópico resposta não encontrado");
        }

        private async Task<ForumTopicoResposta> GetFirstAsync
        (
            SelecionarForumTopicoRespostaByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.ForumTopicoId.Equals(request.Id),
                    cancellationToken,
                    item => item.ForumTopico
                );
        }
    }

    public class SelecionarForumTopicoRespostaByIdQueryValidator : AbstractValidator<SelecionarForumTopicoRespostaByIdQuery>
    {
        public SelecionarForumTopicoRespostaByIdQueryValidator()
        {
            RuleFor(item => item.Id)
                .NotEmpty()
                .WithMessage("Id do fórum resposta vazio");
        }
    }
}
