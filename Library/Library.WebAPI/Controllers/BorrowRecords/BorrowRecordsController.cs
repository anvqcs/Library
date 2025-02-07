using Library.UseCases.Features.BorrowRecords.Commands.CreateBorrowRecord;
using Library.UseCases.Features.BorrowRecords.Commands.DeleteBorrowRecord;
using Library.UseCases.Features.BorrowRecords.Commands.UpdateBorrowRecord;
using Library.UseCases.Features.BorrowRecords.Queries.GetBorrowRecordsByUserWithPagination;
using Library.UseCases.Features.BorrowRecords.Queries.StatisticBorrowRecordsByUserPerMonth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.WebAPI.Controllers.BorrowRecords;

[Route("api/borrow-records")]
[ApiController]
[Authorize]
public class BorrowRecordsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BorrowRecordsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBorrowRecordsByUserWithPagination(
        [FromQuery] GetBorrowRecordsByUserWithPaginationQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
    [HttpGet("statistic")]
    public async Task<IActionResult> GetBorrowRecordsStatistic([FromQuery] StatisticBorrowRecordsByUserPerMonthQuery query)
    {
        return Ok(await _mediator.Send(query));
    }
    [HttpPost]
    public async Task<IActionResult> CreateBorrowRecord([FromBody] CreateBorrowRecordCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBorrowRecord([FromBody] UpdateBorrowRecordCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBorrowRecord(int id)
    {
        var command = new DeleteBorrowRecordCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}