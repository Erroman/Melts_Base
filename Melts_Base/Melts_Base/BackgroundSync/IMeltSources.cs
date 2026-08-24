using Melts_Base.OracleModels;
using Melts_Base.SybaseModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Melts_Base.BackgroundSync
{
    internal interface ISybaseMeltSource
    {
        Task<bool> CanConnectAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<SybaseMelt>> ReadAsync(CancellationToken cancellationToken);
    }

    internal interface IOracleMeltSource
    {
        Task<bool> CanConnectAsync(CancellationToken cancellationToken);
        Task<IReadOnlyList<OracleMelt>> ReadAsync(CancellationToken cancellationToken);
    }
}
