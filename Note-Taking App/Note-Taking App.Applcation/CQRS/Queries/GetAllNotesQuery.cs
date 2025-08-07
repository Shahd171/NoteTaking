using MediatR;
using Note_Taking_App.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Note_Taking_App.Applcation.CQRS.Queries
{
    public record GetAllNotesQuery(string userId) : IRequest<List<NoteDto>>;

}
