using Note_Taking_App.Core.DTOs;
using Note_Taking_App.Core.Entities;
using Note_Taking_App.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Core.Interfaces.Repositories
{
    public interface INoteRepository
    {
        Task<List<NoteDto>> GetAllAsync(string userId);
        Task<NoteDto?> GetByIdAsync(int id, string userId);
        Task<Note?> GetNoteEntityByIdAsync(int id, string userId);

        Task<Note> AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(Note note);
    }
}
