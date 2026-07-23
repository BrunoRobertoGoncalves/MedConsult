using API.Application.Commands.Users;
using Domain.AggregatesModel.UserAggregate.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        public UserController(IUserRepository userRepository, IMediator mediator)
        {
            _userRepository = userRepository;
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand)
        {
            if (createUserCommand == null)
                return BadRequest();

            var userId = await _mediator.Send(createUserCommand);

            if (userId == Guid.Empty)
                return BadRequest();

            return CreatedAtAction(nameof(Add), new { id = userId }, userId);
        }

        [HttpPut]
        [Route("{Id:guid}/disable")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Disable(Guid Id)
        {
            if (Id == Guid.Empty)
                return BadRequest();

            var isOk = await _mediator.Send(new DisableUserCommand(Id));

            if (isOk)
                return Ok();

            return NotFound();
        }

        [HttpPut]
        [Route("{Id:guid}/enable")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Enable(Guid Id)
        {
            if (Id == Guid.Empty)
                return BadRequest();

            var isOk = await _mediator.Send(new EnableUserCommand(Id));

            if (isOk)
                return Ok();

            return NotFound();
        }

    }
}
