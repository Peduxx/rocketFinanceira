using Application.Subscription.Queries;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [Authorize]
        [HttpPost("CreateSubscription")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Response>> CreateSubscription()
        {
            try
            {
                var idUserFromToken = User.FindFirst("IdUser")!.Value;

                var response = await _mediator.Send(new CreateSubscriptionCommand(Guid.Parse(idUserFromToken)));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPatch("CancelSubscription")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Response>> CancelSubscription()
        {
            try
            {
                var idUserFromToken = User.FindFirst("IdUser")!.Value;

                var response = await _mediator.Send(new CancelSubscriptionCommand(Guid.Parse(idUserFromToken)));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetSubscription")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Response>> GetSubscription()
        {
            try
            {
                var idUserFromToken = User.FindFirst("IdUser")!.Value;

                var response = await _mediator.Send(new GetSubscriptionQuery(Guid.Parse(idUserFromToken)));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
