using Assessment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using Assessment.Infrastructure.Models;
using Assessment.Domain.Exceptions;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Assessment.Infrastructure.Services
{
    public class ExternalBalanceService : IExternalBalanceService
    {
        private readonly HttpClient _httpClient;

        public ExternalBalanceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetBalanceAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/balance/{userId}");
            response.EnsureSuccessStatusCode();

            //var balance = await response.Content.ReadAsStringAsync();
            //return Convert.ToDecimal(balance);

            var balanceResponse = await response.Content.ReadFromJsonAsync<BalanceResponse>();
            if (balanceResponse != null && balanceResponse.Success)
            {
                return balanceResponse.Balance;
            }
            return 0;
        }

        public async Task<bool> DebitBalanceAsync(int userId, decimal amount)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/balance/debit", new { UserId = userId, Amount = amount });
            response.EnsureSuccessStatusCode();

            var balanceResponse = await response.Content.ReadAsStringAsync();
            var balance = JsonConvert.DeserializeObject<BalanceResponse>(balanceResponse);
            return balance != null && balance.Success;
        }
    }
}

