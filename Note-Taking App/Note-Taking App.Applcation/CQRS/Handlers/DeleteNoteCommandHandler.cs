using AutoMapper;
using MediatR;
using Note_Taking_App.Applcation.CQRS.Commands;
using Note_Taking_App.Core.Interfaces.Repositories;
using Note_Taking_App.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Applcation.CQRS.Handlers
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
    {
        private readonly INoteRepository NoteRepository;
        private readonly IMapper _mapper;
        public DeleteNoteCommandHandler(INoteRepository noteRepository, IMapper mapper)
        {
            NoteRepository = noteRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var note = await NoteRepository.GetNoteEntityByIdAsync(request.Id, request.UserId);

            if (note == null) return false;
            var Note = _mapper.Map<Note>(note);

            await NoteRepository.DeleteAsync(Note);
            return true;
        }
    }
}
