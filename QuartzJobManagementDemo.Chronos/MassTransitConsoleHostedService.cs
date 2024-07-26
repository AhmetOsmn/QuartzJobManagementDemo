using MassTransit;
using Microsoft.Extensions.Hosting;

namespace QuartzJobManagementDemo.Chronos
{
    public class MassTransitConsoleHostedService(IBusControl bus) : IHostedService
    {
        private readonly IBusControl _bus = bus;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _bus.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _bus.StopAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
