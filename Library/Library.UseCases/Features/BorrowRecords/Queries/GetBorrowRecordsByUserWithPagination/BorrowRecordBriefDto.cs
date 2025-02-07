using AutoMapper;
using Library.Core.Entities;

namespace Library.UseCases.Features.BorrowRecords.Queries.GetBorrowRecordsByUserWithPagination;

public class BorrowRecordBriefDto
{
    public int Id { get; set; }
    public string Book { get; set; } = string.Empty;
    public int BookId { get; set; }
    public string ApplicationUserId { get; set; } = string.Empty;
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public double RentalCost { get; set; }
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<BorrowRecord, BorrowRecordBriefDto>()
                .ForMember(dest => dest.Book, opt => opt.MapFrom(src => src.Book!.Title));

        }
    }
}