using Application.Commands.Users;
using Domain.AggregatesModel.UserAggregate.Repository;
using Domain.Enums;
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

        [HttpPatch]
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

        [HttpPatch]
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

        [HttpPatch]
        [Route("{Id:guid}/updateName")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUserName(Guid Id, string newUserName)
        {
            if (Id == Guid.Empty)
                return BadRequest();

            var isOk = await _mediator.Send(new UpdateUserNameCommand(Id, newUserName));

            if (isOk)
                return Ok();

            return NotFound();
        }

        [HttpPatch]
        [Route("{Id:guid}/updateEmail")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUserEmail(Guid Id, string newUserEmail)
        {
            if (Id == Guid.Empty)
                return BadRequest();

            var isOk = await _mediator.Send(new UpdateUserEmailCommand(Id, newUserEmail));

            if (isOk)
                return Ok();

            return NotFound();
        }

        [HttpPatch]
        [Route("{Id:guid}/updateRole")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateUserRole(Guid Id, UserRole newRole)
        {
            if (Id == Guid.Empty)
                return BadRequest();

            var isOk = await _mediator.Send(new UpdateUserRoleCommand(Id, newRole));

            if (isOk)
                return Ok();

            return NotFound();
        }

        [HttpPatch]
        [Route("{Id:guid}/addSpeciality")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddUserSpeciality(Guid Id, SpecialityType speciality)
        {
            if (Id == Guid.Empty)
                return BadRequest();

            var isOk = await _mediator.Send(new AddUserSpecialityCommand(Id, speciality));

            if (isOk)
                return Ok();

            return NotFound();
        }

        [HttpPatch]
        [Route("{Id:guid}/removeSpeciality")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveUserSpeciality(Guid Id, SpecialityType speciality)
        {
            if (Id == Guid.Empty)
                return BadRequest();

            var isOk = await _mediator.Send(new RemoveUserSpecialityCommand(Id, speciality));

            if (isOk)
                return Ok();

            return NotFound();
        }

    }
}
