using AutoMapper;
using MediatR;
using Note_Taking_App.Applcation.CQRS.Queries;
using Note_Taking_App.Core.DTOs;
using Note_Taking_App.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Applcation.CQRS.Handlers
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, NoteDto>
    {
        private readonly INoteRepository NoteRepository;
        private readonly IMapper _mapper;

        public GetNoteByIdQueryHandler(INoteRepository noteRepository, IMapper mapper)
        {
            NoteRepository = noteRepository;
            _mapper = mapper;
        }

        public async Task<NoteDto> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            var noteDto = await NoteRepository.GetByIdAsync(request.Id, request.userId);
            return noteDto;
        }
    }
}
