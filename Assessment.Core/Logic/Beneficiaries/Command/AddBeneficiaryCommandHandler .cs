using Assessment.Core.Interfaces;
using Assessment.Domain.Exceptions;
using Assessment.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core.Logic.Beneficiaries.Command
{

    public class AddBeneficiaryCommandHandler : IRequestHandler<AddBeneficiaryCommand, int>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;

        public AddBeneficiaryCommandHandler(IUserRepository userRepository, IBeneficiaryRepository beneficiaryRepository)
        {
            _userRepository = userRepository;
            _beneficiaryRepository = beneficiaryRepository;
        }

        public async Task<int> Handle(AddBeneficiaryCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new DomainException("User not found.");
            }

            if (user.Beneficiaries == null)
            {
                user.Beneficiaries = new List<Beneficiary>();
            }
            if (user.Beneficiaries.Count >= 5)
            {
                throw new DomainException("Cannot add more than 5 beneficiaries.");
            }

            var beneficiary = new Beneficiary
            {
                UserId = request.UserId,
                NickName = request.Nickname
            };

            await _beneficiaryRepository.AddAsync(beneficiary);
            await _beneficiaryRepository.SaveChangesAsync();

            return beneficiary.Id;
        }
    }

}
