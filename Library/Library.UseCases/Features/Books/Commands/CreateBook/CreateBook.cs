using System.Security.Claims;
using Library.Core.Entities;
using Library.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Library.UseCases.Features.Books.Commands.CreateBook;

public record CreateBookCommand : IRequest<int>
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public int Year { get; set; }
    public required string ISBN { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
    public double RentalPrice { get; set; }
    public int GenreId { get; set; }
}

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, int>
{
    private readonly IGenericRepository<Book> _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateBookCommandHandler(IGenericRepository<Book> repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<int> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var entity = new Book
        {
            Title = request.Title,
            Author = request.Author,
            Year = request.Year,
            ISBN = request.ISBN,
            Description = request.Description,
            Quantity = request.Quantity,
            RentalPrice = request.RentalPrice,
            GenreId = request.GenreId,
            CreatedBy = userId,
            Created = DateTime.Now
        };
        await _repository.AddAsync(entity, cancellationToken);
        return entity.Id;
    }
}