using System.Security.Claims;
using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using Library.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Library.UseCases.Features.Books.Commands.UpdateBook;

public record UpdateBookCommand : IRequest
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public int Year { get; set; }
    public required string ISBN { get; set; }
    public required string Description { get; set; }
    public int Quantity { get; set; }
    public double RentalPrice { get; set; }
    public int GenreId { get; set; }
}
public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand>
{
    private readonly IGenericRepository<Book> _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateBookCommandHandler(IGenericRepository<Book> repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundException($"{nameof(Book)} with ID {request.Id} not found.");
        }
        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        entity.Title = request.Title;
        entity.Author = request.Author;
        entity.Year = request.Year;
        entity.ISBN = request.ISBN;
        entity.Description = request.Description;
        entity.Quantity = request.Quantity;
        entity.RentalPrice = request.RentalPrice;
        entity.GenreId = request.GenreId;
        entity.ModifiedBy = userId;
        entity.Modified = DateTime.Now;
        await _repository.UpdateAsync(entity, cancellationToken);
    }
}