using System.Security.Claims;
using Library.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Library.UseCases.Features.BorrowRecords.Queries.StatisticBorrowRecordsByUserPerMonth;

public record StatisticBorrowRecordsByUserPerMonthQuery
    (int year) : IRequest<StatisticBorrowRecordsByUserPerMonthResponse>;

public class StatisticBorrowRecordsByUserPerMonthHandler : IRequestHandler<StatisticBorrowRecordsByUserPerMonthQuery,
    StatisticBorrowRecordsByUserPerMonthResponse>
{
    private readonly ILibraryDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public StatisticBorrowRecordsByUserPerMonthHandler(ILibraryDbContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<StatisticBorrowRecordsByUserPerMonthResponse> Handle(
        StatisticBorrowRecordsByUserPerMonthQuery request,
        CancellationToken cancellationToken)
    {
        var result = new StatisticBorrowRecordsByUserPerMonthResponse();
        var userId = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var borrowRecords = await _context.BorrowRecords!
            .Where(br => br.ApplicationUserId == userId && br.BorrowDate.Year == request.year)
            .ToListAsync(cancellationToken);
        for (var i = 1; i <= 12; i++)
        {
            // ReSharper disable once AccessToModifiedClosure
            var records = borrowRecords.Where(br => br.BorrowDate.Month == i);
            result.TotalRentalCost += (double)records.Sum(br => br.RentalCost);
            result.BorrowRecords.Add(new StatisticBorrowRecordsByUserPerMonth
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

public class StatisticBorrowRecordsByUserPerMonth
{
    public string Month { get; set; } = string.Empty;
    public int TotalBorrowed { get; set; }
    public double? TotalRentalCost { get; set; } = 0;
}
public class StatisticBorrowRecordsByUserPerMonthResponse
{
    public List<StatisticBorrowRecordsByUserPerMonth> BorrowRecords { get; set; } =
        new List<StatisticBorrowRecordsByUserPerMonth>();
    public double TotalRentalCost { get; set;} = 0;
}