using Assessment.Core.Interfaces;
using Assessment.Domain.Models;
using Assessment.Domain.Exceptions;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Assessment.Core.Logic.Beneficiaries.Command;
using Assessment.Core.Logic.Beneficiaries.Queries;

namespace Assessment.Tests.Handlers
{
    public class AddBeneficiaryCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IBeneficiaryRepository> _beneficiaryRepositoryMock;
        private readonly AddBeneficiaryCommandHandler _handler;

        public AddBeneficiaryCommandHandlerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _beneficiaryRepositoryMock = new Mock<IBeneficiaryRepository>();
            _handler = new AddBeneficiaryCommandHandler(_userRepositoryMock.Object, _beneficiaryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ShouldThrowDomainException()
        {
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            var command = new AddBeneficiaryCommand { UserId = 1, Nickname = "Test Beneficiary" };

            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<DomainException>()
                .WithMessage("User not found.");
        }

        [Fact]
        public async Task Handle_ExceedsBeneficiaryLimit_ShouldThrowDomainException()
        {
            var user = new User
            {
                Id = 1,
                Beneficiaries = new List<Beneficiary>
                {
                    new Beneficiary { Id = 1 },
                    new Beneficiary { Id = 2 },
                    new Beneficiary { Id = 3 },
                    new Beneficiary { Id = 4 },
                    new Beneficiary { Id = 5 }
                }
            };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

            var command = new AddBeneficiaryCommand { UserId = 1, Nickname = "Test Beneficiary" };

            await FluentActions.Invoking(() => _handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<DomainException>()
                .WithMessage("Cannot add more than 5 beneficiaries.");
        }

    }
}
