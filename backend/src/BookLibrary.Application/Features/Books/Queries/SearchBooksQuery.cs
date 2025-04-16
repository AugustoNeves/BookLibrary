using BookLibrary.Application.Common.Interfaces;
using BookLibrary.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary.Application.Features.Books.Queries
{
    public class SearchBooksQuery : IRequest<IReadOnlyCollection<BookDto>>
    {
        public string? Author { get; set; }
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Category { get; set; }
        public bool OnlyAvailable { get; set; }
    }

    public class SearchBooksQueryHandler(IApplicationDbContext context) : IRequestHandler<SearchBooksQuery, IReadOnlyCollection<BookDto>>
    {
        private readonly IApplicationDbContext _context = context;

        public async Task<IReadOnlyCollection<BookDto>> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(request.Author))
            {
                query = query.Where(b => b.FirstName.Contains(request.Author) || b.LastName.Contains(request.Author));
            }
            if (!string.IsNullOrEmpty(request.ISBN))
            {
                query = query.Where(b => b.ISBN!.Contains(request.ISBN));
            }
            if (!string.IsNullOrEmpty(request.Title))
            {
                query = query.Where(b => b.Title.Contains(request.Title));
            }
            if (!string.IsNullOrEmpty(request.Category))
            {
                query = query.Where(b => b.Category!.Contains(request.Category));
            }
            if (request.OnlyAvailable)
            {
                query = query.Where(b => b.TotalCopies > b.CopiesInUse);
            }            
            
            
            return await query
                .AsNoTracking()
                .Select(b => new BookDto
            {
                BookId = b.BookId,
                Title = b.Title,
                FirstName = b.FirstName,
                LastName = b.LastName,
                TotalCopies = b.TotalCopies,
                CopiesInUse = b.CopiesInUse,
                Type = b.Type,
                ISBN = b.ISBN,
                Category = b.Category
            }).ToListAsync(cancellationToken);
        }
    }   
}
