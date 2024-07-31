using Assessment.Core.Dtos;
using Assessment.Core.Interfaces;
using Assessment.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core.Logic.Topups.Command
{

    public class TopUpCommandHandler : IRequestHandler<TopUpCommand, TopUpResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly ITopUpTransactionRepository _topUpTransactionRepository;
        private readonly IExternalBalanceService _balanceservice;

        public TopUpCommandHandler(
            IUserRepository userRepository,
            IBeneficiaryRepository beneficiaryRepository,
            ITopUpTransactionRepository topUpTransactionRepository,
            IExternalBalanceService balanceService)
        {
            _userRepository = userRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _topUpTransactionRepository = topUpTransactionRepository;
            _balanceservice = balanceService;
        }

        public async Task<TopUpResult> Handle(TopUpCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new TopUpResult { Success = false, Message = "User not found." };
            }
            var beneficiary = user.Beneficiaries.FirstOrDefault(b => b.Id == request.BeneficiaryId);
            if (beneficiary == null || beneficiary.UserId != request.UserId)
            {
                return new TopUpResult { Success = false, Message = "Beneficiary not found or does not belong to the user." };
            }

            var monthlyLimit = user.IsVerified ? 500m : 100m;
            var totalBeneficiaryTopUp = await _topUpTransactionRepository.GetTotalTopUpAmountForBeneficiaryAsync(request.BeneficiaryId, DateTime.Now.Month, DateTime.Now.Year);

            if (totalBeneficiaryTopUp + request.Amount > monthlyLimit)
            {
                return new TopUpResult { Success = false, Message = $"Exceeds monthly limit of {monthlyLimit} AED for this beneficiary." };
            }

            var totalUserTopUp = await _topUpTransactionRepository.GetTotalTopUpAmountForUserAsync(request.UserId, DateTime.Now.Month, DateTime.Now.Year);

            if (totalUserTopUp + request.Amount > 3000m)
            {
                return new TopUpResult { Success = false, Message = "Exceeds monthly limit of 3000 AED for all beneficiaries." };
            }

            var balance = await _balanceservice.GetBalanceAsync(user.Id);
            if (balance < request.Amount + 1) // 1 AED charge
            {
                return new TopUpResult { Success = false, Message = "Insufficient balance." };
            }

            var success = await _balanceservice.DebitBalanceAsync(user.Id, request.Amount + 1);
            if (!success)
            {
                return new TopUpResult { Success = false, Message = "Failed to debit balance." };
            }

            var topUpTransaction = new TopUpTransaction
            {
                UserId = request.UserId,
                BeneficiaryId = request.BeneficiaryId,
                Amount = request.Amount,
                Charge = 1,
                Date = DateTime.Now
            };

            await _topUpTransactionRepository.AddAsync(topUpTransaction);
            await _topUpTransactionRepository.SaveChangesAsync();

            return new TopUpResult { Success = true, Message = "Top-up successful." };
        }
    }
}


