using Melts_Base.OracleModels;
using Melts_Base.SybaseModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Melts_Base.BackgroundSync
{
    internal sealed class RuntimeSybaseMeltSource : ISybaseMeltSource
    {
        private readonly IPollingRuntimeOptions _runtime;
        private readonly RealSybaseMeltSource _real;
        private readonly SqliteFakeSybaseMeltSource _fake;

        public RuntimeSybaseMeltSource(IPollingRuntimeOptions runtime, RealSybaseMeltSource real, SqliteFakeSybaseMeltSource fake)
        {
            _runtime = runtime; _real = real; _fake = fake;
        }

        public Task<IReadOnlyList<SybaseMelt>> ReadAsync(CancellationToken cancellationToken) =>
            _runtime.TestMode ? _fake.ReadAsync(cancellationToken) : _real.ReadAsync(cancellationToken);

        public Task<bool> CanConnectAsync(CancellationToken cancellationToken) =>
            _runtime.TestMode ? Task.FromResult(true) : _real.CanConnectAsync(cancellationToken);
    }

    internal sealed class RuntimeOracleMeltSource : IOracleMeltSource
    {
        private readonly IPollingRuntimeOptions _runtime;
        private readonly RealOracleMeltSource _real;
        private readonly SqliteFakeOracleMeltSource _fake;

        public RuntimeOracleMeltSource(IPollingRuntimeOptions runtime, RealOracleMeltSource real, SqliteFakeOracleMeltSource fake)
        {
            _runtime = runtime; _real = real; _fake = fake;
        }

        public Task<IReadOnlyList<OracleMelt>> ReadAsync(CancellationToken cancellationToken) =>
            _runtime.TestMode ? _fake.ReadAsync(cancellationToken) : _real.ReadAsync(cancellationToken);

        public Task<bool> CanConnectAsync(CancellationToken cancellationToken) =>
            _runtime.TestMode ? Task.FromResult(true) : _real.CanConnectAsync(cancellationToken);
    }
}
