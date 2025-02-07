using System.Reflection;
using Library.Core.Interfaces;
using Library.UseCases.Features.Books.Queries.GetBookById;
using Library.UseCases.Features.Books.Queries.GetBooksWithPagination;
using Library.UseCases.Features.BorrowRecords.Queries.GetBorrowRecordsByUserWithPagination;
using Microsoft.Extensions.DependencyInjection;

namespace Library.UseCases;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAutoMapper(Assembly.GetCallingAssembly());
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<BookDto.Mapping>();
            cfg.AddProfile<BookBriefDto.Mapping>();
            cfg.AddProfile<BorrowRecordBriefDto.Mapping>();
        });

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }
}