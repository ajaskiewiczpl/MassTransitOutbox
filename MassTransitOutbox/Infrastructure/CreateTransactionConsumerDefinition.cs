using MassTransit;

namespace MassTransitOutbox.Infrastructure
{
    public class CreateTransactionConsumerDefinition : ConsumerDefinition<CreateTransactionConsumer>
    {
        public CreateTransactionConsumerDefinition()
        {
            ConcurrentMessageLimit = 1;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateTransactionConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseEntityFrameworkOutbox<AppDbContext>(context);
        }
    }
}
