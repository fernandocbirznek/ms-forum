using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_forum.Extensions;
using ms_forum.Features.ForumTopicoReplicaFeature.Commands;
using ms_forum.Features.ForumTopicoReplicaFeature.Queries;

namespace ms_forum.Features.ForumTopicoReplicaFeature
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumTopicoReplicaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ForumTopicoReplicaController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("inserir")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(InserirForumTopicoReplicaCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpPut("atualizar")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(AtualizarForumTopicoReplicaCommand request)
        {
            return await this.SendAsync(_mediator, request);
        }

        [HttpDelete("excluir/{forumTopicoReplicaId}")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(long forumTopicoReplicaId)
        {
            return await this.SendAsync(_mediator, new RemoverForumTopicoReplicaCommand() { Id = forumTopicoReplicaId });
        }

        [HttpGet("selecionar-forum-replica-by-forum-topico-resposta-id/{forumTopicoReplicaId}")]
        public async Task<ActionResult> GetForumTopicoReplicaByForumTopicoRespostaId(long forumTopicoReplicaId)
        {
            return await this.SendAsync(_mediator, new SelecionarForumTopicoReplicaByForumTopicoRespostaIdFiltersQuery() { Id = forumTopicoReplicaId });
        }

        [HttpGet("selecionar-forum/{forumReplicaId}")]
        public async Task<ActionResult> GetForum(long forumReplicaId)
        {
            return await this.SendAsync(_mediator, new SelecionarForumTopicoReplicaByIdQuery() { Id = forumReplicaId });
        }

        [HttpGet("selecionar-forum-replicas")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarForumTopicoReplicaFiltersQuery());
        }
    }
}
