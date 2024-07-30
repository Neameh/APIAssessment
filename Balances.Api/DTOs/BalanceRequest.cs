namespace Balances.Api.DTOs
{
    public class BalanceRequest
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
