using AutoMapper;
using MediatR;
using Note_Taking_App.Applcation.CQRS.Commands;
using Note_Taking_App.Core.DTOs;
using Note_Taking_App.Core.Interfaces.Repositories;
using Note_Taking_App.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Applcation.CQRS.Handlers
{
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly INoteRepository NoteRepository;

        public UpdateNoteCommandHandler(INoteRepository noteRepository, IMapper mapper)
        {
            NoteRepository = noteRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            var noteEntity = await NoteRepository.GetNoteEntityByIdAsync(request.Id, request.UserId);
            if (noteEntity == null) return false;

            _mapper.Map(request.Dto, noteEntity); 
            noteEntity.UpdatedAt = DateTime.Now;

            await NoteRepository.UpdateAsync(noteEntity);
            return true;
        }
    }
}
