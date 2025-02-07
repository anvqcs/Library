using System.Security.Claims;
using Library.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.UseCases.Features.BorrowRecords.Queries.StatisticBorrowRecordsByUserPerMonth;

public record StatisticBorrowRecordsByUserPerMonthQuery
    (int year) : IRequest<List<StatisticBorrowRecordsByUserPerMonthResponse>>;

public class StatisticBorrowRecordsByUserPerMonthHandler : IRequestHandler<StatisticBorrowRecordsByUserPerMonthQuery,
    List<StatisticBorrowRecordsByUserPerMonthResponse>>
{
    private readonly ILibraryDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StatisticBorrowRecordsByUserPerMonthHandler(ILibraryDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<StatisticBorrowRecordsByUserPerMonthResponse>> Handle(
        StatisticBorrowRecordsByUserPerMonthQuery request,
        CancellationToken cancellationToken)
    {
        var result = new List<StatisticBorrowRecordsByUserPerMonthResponse>();
        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var borrowRecords = await _context.BorrowRecords!
            .Where(br => br.ApplicationUserId == userId && br.BorrowDate.Year == request.year)
            .ToListAsync(cancellationToken);
        for (var i = 1; i <= 12; i++)
        {
            // ReSharper disable once AccessToModifiedClosure
            var records = borrowRecords.Where(br => br.BorrowDate.Month == i);
            result.Add(new StatisticBorrowRecordsByUserPerMonthResponse
            {
                Month = new DateTime(request.year, i, 1).ToString("MMMM"),
                // ReSharper disable once PossibleMultipleEnumeration
                TotalBorrowed = records.Count(),
                // ReSharper disable once PossibleMultipleEnumeration
                TotalRentalCost = records.Sum(br => br.RentalCost)
            });
        }

        return result;
    }
}

public class StatisticBorrowRecordsByUserPerMonthResponse
{
    public string Month { get; set; } = string.Empty;
    public int TotalBorrowed { get; set; }
    public double? TotalRentalCost { get; set; } = 0;
}