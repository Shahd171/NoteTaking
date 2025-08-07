using MediatR;
using Note_Taking_App.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Applcation.CQRS.Commands
{
    public record DeleteNoteCommand : IRequest<bool>
    {
        public int Id { get; }
        public string UserId { get; }

        public DeleteNoteCommand(int id, string userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
