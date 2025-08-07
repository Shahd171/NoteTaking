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
    public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, List<NoteDto>>
    {
        private readonly INoteRepository _repo;
        private readonly IMapper _mapper;

        public GetAllNotesQueryHandler(INoteRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<NoteDto>> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
        {
            var notes = await _repo.GetAllAsync(request.userId);
            return _mapper.Map<List<NoteDto>>(notes);
        }
    }
}
