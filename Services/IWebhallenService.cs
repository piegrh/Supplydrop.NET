using System.Threading;
using System.Threading.Tasks;
using Webhallen.Models;

namespace Webhallen.Services
{
    public interface IWebhallenService
    {
        Task<LoginResponse?> Login(LoginRequest request,CancellationToken ct = default);

        Task<SupplyDropResponse?> SupplyDrop(CancellationToken ct = default);

        Task<MeResponse?> Me(CancellationToken ct = default);
    }
}
