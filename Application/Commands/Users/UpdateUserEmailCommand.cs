using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.Users
{
    public class UpdateUserEmailCommand : IRequest<bool>
    {
        public UpdateUserEmailCommand(Guid id, string email)
        {
            Id = id;
            Email = email;
        }

        public Guid Id { get; private set; }

        public string Email { get; private set; }
    }
}
