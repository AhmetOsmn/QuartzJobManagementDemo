using MassTransit;

namespace QuartzJobManagementDemo.Shared
{
    public class MasstransitConfiguration
    {
        public static IBusControl ConfigureBus(Action<IRabbitMqBusFactoryConfigurator>? registration = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {

                cfg.Host(new Uri(RmqConfig.RmqUri), h =>
                {
                    h.Username(RmqConfig.RmqUserName);
                    h.Password(RmqConfig.RmqPassword);
                });

                registration?.Invoke(cfg);
            });
        }
    }
}
