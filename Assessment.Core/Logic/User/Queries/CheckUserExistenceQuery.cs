using MediatR;

public class CheckUserExistenceQuery : IRequest<bool>
{
    public int UserId { get; set; }
}
