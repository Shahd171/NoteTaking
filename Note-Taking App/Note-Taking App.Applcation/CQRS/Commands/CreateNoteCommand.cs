using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Note_Taking_App.Core.DTOs;

namespace Note_Taking_App.Applcation.CQRS.Commands
{
    public class CreateNoteCommand : IRequest<NoteDto>
    {
        public CreateNoteDto Dto { get; }
        public string UserId { get; }

        public CreateNoteCommand(CreateNoteDto dto, string userId)
        {
            Dto = dto;
            UserId = userId;

        }
    }



}
