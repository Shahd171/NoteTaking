using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Note_Taking_App.Applcation.CQRS.Commands;
using Note_Taking_App.Applcation.CQRS.Queries;
using Note_Taking_App.Core.DTOs;
using Note_Taking_App.ResponseModel;
using System.Security.Claims;

namespace Note_Taking_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize]

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNoteDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                Console.WriteLine(userId);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found in token.");

                var result = await _mediator.Send(new CreateNoteCommand(dto, userId));
                return Ok(ApiResponse<object>.SuccessResponse(result, "Note has been created successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message, "Failed to create the note."));
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found in token.");

                var result = await _mediator.Send(new GetAllNotesQuery(userId));
                return Ok(ApiResponse<object>.SuccessResponse(result, "Notes retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message, "An error occurred while retrieving notes."));
            }
        }
        [Authorize]

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found in token.");

                var result = await _mediator.Send(new GetNoteByIdQuery(id,userId));
                if (result == null)
                    return NotFound(ApiResponse<object>.ErrorResponse("Note not found.", "The requested note could not be found."));

                return Ok(ApiResponse<object>.SuccessResponse(result, "Note retrieved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message, "An error occurred while retrieving the note."));
            }
        }
        [Authorize]

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found in token.");

                var result = await _mediator.Send(new UpdateNoteCommand(id, dto,userId));
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Note not found.", "The note to update was not found."));

                return Ok(ApiResponse<object>.SuccessResponse(result, "Note has been updated successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message, "An error occurred while updating the note."));
            }
        }
        [Authorize]

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User ID not found in token.");

                var result = await _mediator.Send(new DeleteNoteCommand(id, userId));
                if (!result)
                    return NotFound(ApiResponse<object>.ErrorResponse("Note not found.", "The note to delete was not found."));

                return Ok(ApiResponse<object>.SuccessResponse(null, "Note has been deleted successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message, "An error occurred while deleting the note."));
            }
        }
    }
}