namespace miniBankingAPI.Application.DTOs
{
    public record CreateAccountRequest(int CustomerId, string CurrencyType);
    public record CreateAccountResponse(int AccountId, string AccountNumber);

    public record GetAccountBalanceResponse(int AccountId, string AccountNumber, decimal Balance, string CurrencyType, bool IsActive);

    public record TransferMoneyRequest(int FromAccountId, int ToAccountId, decimal Amount, string? Description);
    public record TransferMoneyResponse(bool Success, string Message);
}
