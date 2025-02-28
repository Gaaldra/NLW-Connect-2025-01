using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure.DataAccess;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts;

public class RegisterBookCheckoutUseCase (LoggedUserService loggedUser)
{
    private const int MAX_LOAN_DAYS = 7;
    private readonly LoggedUserService _loggedUser = loggedUser;

    public void Execute(Guid bookId)
    {
        TechLibraryDbContext dbContext = new();

        Validate(dbContext, bookId);

        User user = _loggedUser.GetUser();

        Checkout entity = new()
        {
            UserId = user.Id,
            BookId = bookId,
            ExpectedReturnDate = DateTime.UtcNow.AddDays(MAX_LOAN_DAYS),
        };

        dbContext.Checkouts.Add(entity);

        dbContext.SaveChanges();
    }

    private static void Validate(TechLibraryDbContext dbContext, Guid bookId)
    {
        var book = dbContext.Books.FirstOrDefault(book => book.Id.Equals(bookId)) ?? throw new NotFoundException("Livro não encontrado");
        
        var amountBookNotReturned = dbContext.Checkouts.Count(checkout => checkout.BookId == bookId && checkout.ReturnedDate == null);

        if (amountBookNotReturned == book.Amount) throw new ConflictException("Livro não está disponivel para emprestimo");
    }
}
