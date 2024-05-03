using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_forum.Extensions;
using ms_forum.Features.ForumTagFeature.Commands;
using ms_forum.Features.ForumTagFeature.Queries;

namespace ms_forum.Features.ForumTagFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumTagController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ForumTagController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirForumTagCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarForumTagCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{forumTagId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long forumTagId)
        {
            return await this.SendAsync(_mediator, new RemoverForumTagCommand() { Id = forumTagId });
        }

        [HttpGet("selecionar-forum-tag/{forumTagId}")]
        public async Task<ActionResult> GetForum(long forumTagId)
        {
            return await this.SendAsync(_mediator, new SelecionarForumTagByIdQuery() { Id = forumTagId });
        }

        [HttpGet("selecionar-forum-tag-sistema")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarForumTagFiltersQuery());
        }
    }
}
