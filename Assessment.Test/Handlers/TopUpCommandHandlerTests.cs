using Assessment.Core.Interfaces;
using Assessment.Domain.Models;
using FluentAssertions;
using Moq;
using Xunit;
using Assessment.Core.Logic.Topups.Command;

namespace Assessment.Tests.Handlers
{
    public class TopUpCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _usersRepositoryMock;
        private readonly Mock<ITopUpTransactionRepository> _topUpRepositoryMock;
        private readonly Mock<IExternalBalanceService> _balanceClientMock;
        private readonly Mock<IBeneficiaryRepository> _beneficiaryRepositoryMock;
        private readonly TopUpCommandHandler _handler;

        public TopUpCommandHandlerTests()
        {
            _usersRepositoryMock = new Mock<IUserRepository>();
            _topUpRepositoryMock = new Mock<ITopUpTransactionRepository>();
            _balanceClientMock = new Mock<IExternalBalanceService>();
            _beneficiaryRepositoryMock = new Mock<IBeneficiaryRepository>();
            _handler = new TopUpCommandHandler(
                _usersRepositoryMock.Object,
                _beneficiaryRepositoryMock.Object,
                _topUpRepositoryMock.Object,
                _balanceClientMock.Object);
        }

        [Fact]
        public async Task Handle_UserNotFound_ShouldReturnFailure()
        {
            _usersRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            var command = new TopUpCommand { UserId = 1, BeneficiaryId = 1, Amount = 100 };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("User not found.");
        }

        [Fact]
        public async Task Handle_BeneficiaryNotFound_ShouldReturnFailure()
        {
            var user = new User { Id = 1, Beneficiaries = new Beneficiary[0] };
            _usersRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);

            var command = new TopUpCommand { UserId = 1, BeneficiaryId = 1, Amount = 100 };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Beneficiary not found or does not belong to the user.");
        }

        [Fact]
        public async Task Handle_ExceedsMonthlyLimitForBeneficiary_ShouldReturnFailure()
        {
            var user = new User { Id = 1, IsVerified = false, Beneficiaries = new[] { new Beneficiary { Id = 1, UserId = 1 } } };
            _usersRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _topUpRepositoryMock.Setup(r => r.GetTotalTopUpAmountForBeneficiaryAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(90);

            var command = new TopUpCommand { UserId = 1, BeneficiaryId = 1, Amount = 20 };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Exceeds monthly limit of 100 AED for this beneficiary.");
        }

        [Fact]
        public async Task Handle_ExceedsMonthlyLimitForUser_ShouldReturnFailure()
        {
            var user = new User { Id = 1, IsVerified = true, Beneficiaries = new[] { new Beneficiary { Id = 1, UserId = 1 } } };
            _usersRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _topUpRepositoryMock.Setup(r => r.GetTotalTopUpAmountForUserAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(2990);

            var command = new TopUpCommand { UserId = 1, BeneficiaryId = 1, Amount = 20 };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Exceeds monthly limit of 3000 AED for all beneficiaries.");
        }

        [Fact]
        public async Task Handle_InsufficientBalance_ShouldReturnFailure()
        {
            var user = new User { Id = 1, IsVerified = true, Beneficiaries = new[] { new Beneficiary { Id = 1, UserId = 1 } } };
            _usersRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _topUpRepositoryMock.Setup(r => r.GetTotalTopUpAmountForUserAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(0);
            _balanceClientMock.Setup(s => s.GetBalanceAsync(It.IsAny<int>())).ReturnsAsync(50);

            var command = new TopUpCommand { UserId = 1, BeneficiaryId = 1, Amount = 100 };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Insufficient balance.");
        }

        [Fact]
        public async Task Handle_DebitBalanceFails_ShouldReturnFailure()
        {
            var user = new User { Id = 1, IsVerified = true, Beneficiaries = new[] { new Beneficiary { Id = 1, UserId = 1 } } };
            _usersRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _topUpRepositoryMock.Setup(r => r.GetTotalTopUpAmountForUserAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(0);
            _balanceClientMock.Setup(s => s.GetBalanceAsync(It.IsAny<int>())).ReturnsAsync(1000);
            _balanceClientMock.Setup(s => s.DebitBalanceAsync(It.IsAny<int>(), It.IsAny<decimal>())).ReturnsAsync(false);

            var command = new TopUpCommand { UserId = 1, BeneficiaryId = 1, Amount = 100 };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Message.Should().Be("Failed to debit balance.");
        }

        [Fact]
        public async Task Handle_SuccessfulTopUp_ShouldReturnSuccess()
        {
            var user = new User { Id = 1, IsVerified = true, Beneficiaries = new[] { new Beneficiary { Id = 1, UserId = 1 } } };
            _usersRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _topUpRepositoryMock.Setup(r => r.GetTotalTopUpAmountForUserAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(0);
            _balanceClientMock.Setup(s => s.GetBalanceAsync(It.IsAny<int>())).ReturnsAsync(1000);
            _balanceClientMock.Setup(s => s.DebitBalanceAsync(It.IsAny<int>(), It.IsAny<decimal>())).ReturnsAsync(true);

            var command = new TopUpCommand { UserId = 1, BeneficiaryId = 1, Amount = 100 };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Success.Should().BeTrue();
            result.Message.Should().Be("Top-up successful.");
            _topUpRepositoryMock.Verify(r => r.AddAsync(It.IsAny<TopUpTransaction>()), Times.Once);
            _topUpRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
