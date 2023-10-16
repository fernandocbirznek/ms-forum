using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_forum.Extensions;
using ms_forum.Features.ForumTopicoFeature.Commands;
using ms_forum.Features.ForumTopicoFeature.Queries;

namespace ms_forum.Features.ForumTopicoFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumTopicoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ForumTopicoController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirForumTopicoCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarForumTopicoCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{ForumtopicoId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long forumId)
        {
            return await this.SendAsync(_mediator, new RemoverForumTopicoCommand() { Id = forumId });
        }

        [HttpGet("selecionar-topico-forum/{ForumtopicoId}")]
        public async Task<ActionResult> GetForum(long ForumtopicoId)
        {
            return await this.SendAsync(_mediator, new SelecionarForumTopicoByIdQuery() { Id = ForumtopicoId });
        }

        [HttpGet("selecionar-topicos-forum")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarForumTopicoFiltersQuery());
        }
    }
}
