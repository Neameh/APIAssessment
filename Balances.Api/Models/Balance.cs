namespace Balances.Api.Models
{
    public class Balance
    {
          
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}

