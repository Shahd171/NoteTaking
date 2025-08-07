using MediatR;
using Note_Taking_App.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Applcation.CQRS.Commands
{
    public class UpdateNoteCommand : IRequest<bool>
    {
        public int Id { get; }
        public UpdateNoteDto Dto { get; }
        public string UserId { get; }

        public UpdateNoteCommand(int id, UpdateNoteDto dto, string userId)
        {
            Id = id;
            Dto = dto;
            UserId = userId;
        }
    }

}
