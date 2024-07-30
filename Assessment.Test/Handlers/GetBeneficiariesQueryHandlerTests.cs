using Assessment.Core.Interfaces;
using Assessment.Core.Logic.Beneficiaries.Queries;
using Assessment.Core.Dtos;
using Assessment.Domain.Models;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Assessment.Tests.Handlers
{
    public class GetBeneficiariesQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetBeneficiariesQueryHandler _handler;

        public GetBeneficiariesQueryHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            //_handler = new GetBeneficiariesQueryHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldReturnBeneficiaries()
        {
            var beneficiaries = new List<Beneficiary>
            {
                new Beneficiary { Id = 1, NickName = "Beneficiary 1" },
                new Beneficiary { Id = 2, NickName = "Beneficiary 2" }
            };
            var user = new User { Id = 1, Beneficiaries = beneficiaries };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

            var query = new GetBeneficiariesQuery { UserId = 1 };

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(2);
            result.Should().Contain(b => b.Id == 1 && b.Nickname == "Beneficiary 1");
            result.Should().Contain(b => b.Id == 2 && b.Nickname == "Beneficiary 2");
        }
    }
}
