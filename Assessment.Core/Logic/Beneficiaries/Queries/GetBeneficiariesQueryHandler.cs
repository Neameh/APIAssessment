using Assessment.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment.Core.Logic.Beneficiaries.Queries
{
    public class GetBeneficiariesQueryHandler : IRequestHandler<GetBeneficiariesQuery, List<BeneficiaryDto>>
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IUserRepository _userRepository;


        public GetBeneficiariesQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //public async Task<List<BeneficiaryDto>> Handle(GetBeneficiariesQuery request, CancellationToken cancellationToken)
        //{
        //    var beneficiaries = await _beneficiaryRepository.GetByUserIdAsync(request.UserId);
        //    return beneficiaries.Select(b => new BeneficiaryDto
        //    {
        //        Id = b.Id,
        //        Nickname = b.NickName
        //    }).ToList();
        //}

        public async Task<List<BeneficiaryDto>> Handle(GetBeneficiariesQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null || user.Beneficiaries == null)
            {
                return new List<BeneficiaryDto>();
            }

            return user.Beneficiaries.Select(b => new BeneficiaryDto
            {
                Id = b.Id,
                Nickname = b.NickName
            }).ToList();
        }
    }
}
