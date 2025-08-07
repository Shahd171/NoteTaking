using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Note_Taking_App.Core.DTOs;
using Note_Taking_App.Core.Entities;
using Note_Taking_App.Core.Interfaces.Repositories;
using Note_Taking_App.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Note_Taking_App.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly NotesDbContext _context;
       
        private readonly AppDbContext _userContext; // Contains Users
        private readonly IMapper _mapper;
        public NoteRepository(NotesDbContext context, AppDbContext userContext, IMapper mapper)
        {
            _context = context;
            _userContext = userContext;
            _mapper = mapper;
        }

        public async Task<List<NoteDto>> GetAllAsync(string userId)
        {
            var notes = await _context.Notes
                .Where(n => n.UserId == userId)
                .ToListAsync();

            var usernames = await _userContext.Users
                .Where(u => u.Id == userId)
                .Select(u => new { u.Id, u.UserName })
                .ToDictionaryAsync(u => u.Id, u => u.UserName);

            var noteDtos = _mapper.Map<List<NoteDto>>(notes);

            foreach (var dto in noteDtos)
            {
                dto.Username = usernames.ContainsKey(userId) ? usernames[userId] : string.Empty;
            }

            return noteDtos;
        }



        public async Task<NoteDto> GetByIdAsync(int id, string userId)
        {
            var note = await _context.Notes
         .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

            if (note == null)
                return null;

            var username = await _userContext.Users
                .Where(u => u.Id == userId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            var noteDto = _mapper.Map<NoteDto>(note);
            noteDto.Username = username ?? string.Empty;

            return noteDto;
        }



        public async Task<Note> AddAsync(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task UpdateAsync(Note note)
        {
            var existing = _context.Notes.Where(n => n.UserId.Equals(note.UserId)).FirstOrDefault();
            _context.Notes.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Note note)
        {
            var existing = _context.Notes.Where(n => n.UserId.Equals(note.UserId));
            _context.Notes.Remove(note);

            await _context.SaveChangesAsync();
        }
        public async Task<Note?> GetNoteEntityByIdAsync(int id, string userId)
        {
            return await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
        }

    }

}
