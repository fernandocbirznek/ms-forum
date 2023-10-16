using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_forum.Extensions;
using ms_forum.Features.ForumFeature.Commands;
using ms_forum.Features.ForumFeature.Queries;

namespace ms_forum.Features.ForumFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ForumController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirForumCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarForumCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{forumId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long forumId)
        {
            return await this.SendAsync(_mediator, new RemoverForumCommand() { Id = forumId });
        }

        [HttpGet("selecionar-forum/{forumId}")]
        public async Task<ActionResult> GetForum(long forumId)
        {
            return await this.SendAsync(_mediator, new SelecionarForumByIdQuery() { Id = forumId });
        }

        [HttpGet("selecionar-foruns")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarForumFiltersQuery());
        }
    }
}
