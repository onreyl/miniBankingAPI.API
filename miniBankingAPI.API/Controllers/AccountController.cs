using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniBankingAPI.Application.Features.Accounts.Commands.CreateAccount;
using miniBankingAPI.Application.Features.Accounts.Commands.TransferMoney;
using miniBankingAPI.Application.Features.Accounts.Queries.GetAccountBalance;

namespace miniBankingAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountBalance(int id)
        {
            var query = new GetAccountBalanceQuery { AccountId = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPost("transfer")]
        public async Task<IActionResult> TransferMoney([FromBody] TransferMoneyCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
