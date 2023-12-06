using MassTransit;

namespace MassTransitOutbox.Infrastructure
{
    public class CreateTransactionConsumerDefinition : ConsumerDefinition<CreateTransactionConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateTransactionConsumer> consumerConfigurator, IRegistrationContext context)
        {
            consumerConfigurator.ConcurrentMessageLimit = 1;
            endpointConfigurator.UseEntityFrameworkOutbox<AppDbContext>(context);
        }
    }
}
