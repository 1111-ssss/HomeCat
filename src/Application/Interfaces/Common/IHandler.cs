using Domain.Model.Result;

namespace Application.Interfaces.Common;

public interface IHandler<Tin, Tout>
where Tin : IRequest<Tout>
{
    Task<Result<Tout>> Handle(Tin request, CancellationToken ct);
}