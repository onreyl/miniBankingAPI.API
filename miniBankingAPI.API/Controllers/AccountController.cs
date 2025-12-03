using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miniBankingAPI.Application.DTOs;
using miniBankingAPI.Application.Features.Accounts.Commands.CreateAccount;
using miniBankingAPI.Application.Features.Accounts.Commands.TransferMoney;
using miniBankingAPI.Application.Features.Accounts.Queries.GetAccountBalance;
using miniBankingAPI.Domain.Enums;

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
            var balance = await _mediator.Send(query);
            
            var response = new GetAccountBalanceResponse(
                AccountId: id,
                AccountNumber: "",
                Balance: balance,
                CurrencyType: "",
                IsActive: true
            );
            
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            var command = new CreateAccountCommand
            {
                CustomerId = request.CustomerId,
                CurrencyType = Enum.Parse<CurrencyType>(request.CurrencyType)
            };
            
            var accountId = await _mediator.Send(command);
            
            var response = new CreateAccountResponse(
                AccountId: accountId,
                AccountNumber: ""
            );
            
            return Ok(response);
        }
        
        [HttpPost("transfer")]
        public async Task<IActionResult> TransferMoney([FromBody] TransferMoneyRequest request)
        {
            var command = new TransferMoneyCommand
            {
                FromAccountId = request.FromAccountId,
                ToAccountId = request.ToAccountId,
                Amount = request.Amount,
                Description = request.Description
            };
            
            var success = await _mediator.Send(command);
            
            var response = new TransferMoneyResponse(
                Success: success,
                Message: success ? "Transfer completed successfully" : "Transfer failed"
            );
            
            return Ok(response);
        }

    }
}
