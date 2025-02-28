using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.UseCases.Books.Filter;

public class FilterBookUseCase
{
    private const int PAGE_SIZE = 10;

    public ResponseBooksJson Execute(RequestFilterBooksJson request)
    {
        TechLibraryDbContext dbContext = new();

        var query = dbContext.Books.AsQueryable();
        int totalCount = 0;

        if (!string.IsNullOrWhiteSpace(request.Title))
        {
            query = query.Where(book => book.Title.ToLower().Contains(request.Title.ToLower()));
            totalCount = dbContext.Books.Count(book => book.Title.ToLower().Contains(request.Title!.ToLower()));
        }
        else
        {
            totalCount = dbContext.Books.Count();
        }

        var books = query
            .OrderBy(book => book.Title)
            .ThenBy(book => book.Author)
            .Skip((request.PageNumber - 1) * PAGE_SIZE)
            .Take(PAGE_SIZE)
            .ToList();

        return new ResponseBooksJson
        {
            Pagination = new ResponsePaginationJson 
            { 
                TotalCount = totalCount,
                PageNumber = request.PageNumber
            },
            Books = books.Select(book => new ResponseBookJson
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author
            }).ToList()
        };
    }
}
