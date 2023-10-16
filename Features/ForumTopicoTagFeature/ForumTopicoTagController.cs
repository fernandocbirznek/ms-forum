using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms_forum.Extensions;
using ms_forum.Features.ForumTopicoTagFeature.Commands;
using ms_forum.Features.ForumTopicoTagFeature.Queries;

namespace ms_forum.Features.ForumTopicoTagFeature
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ForumTopicoTagController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public ForumTopicoTagController
        (
            IMediator mediator,
            IConfiguration configuration
        )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirForumTopicoTagCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarForumTopicoTagCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{forumTopicoTagId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long forumTopicoTagId)
        {
            return await this.SendAsync(_mediator, new RemoverForumTopicoTagCommand() { Id = forumTopicoTagId });
        }

        [HttpGet("selecionar-forum-topico-tag/{forumTopicoTagId}")]
        public async Task<ActionResult> GetForum(long forumTopicoTagId)
        {
            return await this.SendAsync(_mediator, new SelecionarForumTopicoTagByIdQuery() { Id = forumTopicoTagId });
        }

        [HttpGet("selecionar-forum-tags")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarForumTopicoTagFiltersQuery());
        }

        [HttpGet("selecionar-forum-topico-tags/{forumTopicoId}")]
        public async Task<ActionResult> GetTopicoTags(long forumTopicoId)
        {
            return await this.SendAsync(_mediator, new SelecionarForumTopicoTagByTopicoIdQuery() { Id = forumTopicoId });
        }
    }
}
