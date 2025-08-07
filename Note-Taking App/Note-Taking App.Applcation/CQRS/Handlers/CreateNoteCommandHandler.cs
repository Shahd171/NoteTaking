using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Note_Taking_App.Applcation.CQRS.Commands;
using Note_Taking_App.Core.DTOs;
using Note_Taking_App.Infrastructure;
using Note_Taking_App.Core.Entities;
using Note_Taking_App.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Note_Taking_App.Applcation.CQRS.Handlers
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, NoteDto>
    {
        private readonly IMapper Mapper;
        private readonly INoteRepository NoteRepository;
        private readonly UserManager<ApplicationUser> UserManager;

        public CreateNoteCommandHandler(INoteRepository noteRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            NoteRepository = noteRepository;
            Mapper = mapper;
            UserManager = userManager;

        }

        public async Task<NoteDto> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            var user = await UserManager.FindByIdAsync(request.UserId);
            if (user == null) throw new Exception("User not found");

            var note = Mapper.Map<Note>(request.Dto);
            note.UserId = request.UserId;
            note.CreatedAt = DateTime.Now;

            await NoteRepository.AddAsync(note);

            var NoteDto= Mapper.Map<NoteDto>(note);
            NoteDto.Username = user.UserName;
            return NoteDto;
        }
    }
}
