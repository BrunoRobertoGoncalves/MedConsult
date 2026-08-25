using Application.Commands.Users;
using Application.Queries.Users;
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
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{Id:guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(Id));

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll([FromQuery] bool? activeOnly = null)
        {
            var users = await _mediator.Send(new GetAllUsersQuery(activeOnly));

            return Ok(users);
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
