using Assessment.Core.Interfaces;
using MediatR;

public class CheckUserExistenceQueryHandler : IRequestHandler<CheckUserExistenceQuery, bool>
{
    private readonly IUserRepository _usersRepository;

    public CheckUserExistenceQueryHandler(IUserRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<bool> Handle(CheckUserExistenceQuery request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByIdAsync(request.UserId);
        return user != null;
    }
}
