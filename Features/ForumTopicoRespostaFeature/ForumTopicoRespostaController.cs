using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_forum.Extensions;
using ms_forum.Features.ForumTopicoRespostaFeature.Commands;
using ms_forum.Features.ForumTopicoRespostaFeature.Queries;

namespace ms_forum.Features.ForumTopicoRespostaFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumTopicoRespostaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ForumTopicoRespostaController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirForumTopicoRespostaCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarForumTopicoRespostaCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{forumRespostaId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long forumRespostaId)
        {
            return await this.SendAsync(_mediator, new RemoverForumTopicoRespostaCommand() { Id = forumRespostaId });
        }

        [HttpGet("selecionar-forum/{forumRespostaId}")]
        public async Task<ActionResult> GetForum(long forumRespostaId)
        {
            return await this.SendAsync(_mediator, new SelecionarForumTopicoRespostaByIdQuery() { Id = forumRespostaId });
        }

        [HttpGet("selecionar-forum-respostas")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarForumTopicoRespostaFiltersQuery());
        }

        [HttpGet("selecionar-forum-respostas/{topicoId}")]
        public async Task<ActionResult> GetTopicoRespostas(long topicoId)
        {
            return await this.SendAsync(_mediator, new SelecionarForumTopicoRespostaByIdQuery() { Id = topicoId });
        }
    }
}
