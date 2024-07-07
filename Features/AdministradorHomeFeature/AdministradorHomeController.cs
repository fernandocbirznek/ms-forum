using MediatR;
using Microsoft.AspNetCore.Mvc;
using ms_forum.Extensions;
using ms_forum.Features.AdministradorHomeFeature.Queries;

namespace ms_forum.Features.AdministradorHomeFeature
{
    [ApiController]
    [Route("api/administrador-home")]
    public class AdministradorHomeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdministradorHomeController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("selecionar-forum-informacao")]
        public async Task<ActionResult> Get()
        {
            return await this.SendAsync(_mediator, new SelecionarForumInformacaoSistemaQuery());
        }
    }
}
