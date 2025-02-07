using AutoMapper;
using Library.Core.Entities;

namespace Library.UseCases.Features.Books.Queries.GetBookById;

public class BookDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int Year { get; set; }
    public string? ISBN { get; set; }
    public string? Description { get; set; }
    public int Quantity { get; set; }
    public int GenreId { get; set; }
    public double RentalPrice { get; set; }
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime Modified { get; set; }
    public string? ModifiedBy { get; set; }
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.Genre!.Id));
        }
    }
}