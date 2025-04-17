using BookLibrary.Application.Features.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string? author, [FromQuery] string? isbn, [FromQuery] string? title, [FromQuery] string? category, [FromQuery] bool available = true)
        {
            var query = new SearchBooksQuery
            {
                Author = author,
                ISBN = isbn,
                Title = title,
                Category = category,
                OnlyAvailable = available
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }

}
