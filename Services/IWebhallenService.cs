using System.Threading;
using System.Threading.Tasks;
using Webhallen.Models;

namespace Webhallen.Services
{
    public interface IWebhallenService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request,CancellationToken ct = default);

        Task<SupplyDropResponse?> SupplyDropAsync(CancellationToken ct = default);

        Task<MeResponse?> MeAsync(CancellationToken ct = default);
    }
}
