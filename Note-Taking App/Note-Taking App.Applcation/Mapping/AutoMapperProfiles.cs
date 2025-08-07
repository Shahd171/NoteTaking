using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Note_Taking_App.Core.DTOs;
using Note_Taking_App.Core.Entities;
using Note_Taking_App.Infrastructure;
namespace Note_Taking_App.Applcation.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Note, NoteDto>()
             .ForMember(dest => dest.Username, opt => opt.Ignore()).ReverseMap();

            CreateMap<Note, CreateNoteDto>().ReverseMap();
            CreateMap<UpdateNoteDto, Note>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
          
        }
    }
}
