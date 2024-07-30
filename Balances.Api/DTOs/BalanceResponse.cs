namespace Balances.Api.DTOs
{
    namespace ExternalBalanceService.Models
    {
        public class BalanceResponse
        {
            public BalanceResponse()
            {
                Message = "";
            }
            public int UserId { get; set; }
            public decimal Balance { get; set; }
            public bool Success { get; set; }
            public string Message { get; set; }
        }
    }

}
