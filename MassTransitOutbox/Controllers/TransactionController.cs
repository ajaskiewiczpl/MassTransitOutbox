using MassTransit;
using MassTransitOutbox.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitOutbox.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IPublishEndpoint _publisher;
        private readonly ILogger<TransactionController> _logger;
        private readonly AppDbContext _appDbContext;

        public TransactionController(ILogger<TransactionController> logger, IPublishEndpoint publisher, AppDbContext appDbContext)
        {
            _logger = logger;
            _publisher = publisher;
            _appDbContext = appDbContext;
        }

        [HttpPost(Name = "GenerateRandom")]
        public async Task GenerateRandomTransactions()
        {
            await _publisher.Publish(new CreateTransaction(10, "My Account 1"));
            await _publisher.Publish(new CreateTransaction(10, "My Account 1"));
            await _publisher.Publish(new CreateTransaction(10, "My Account 1"));
            await _publisher.Publish(new CreateTransaction(10, "My Account 2"));
            await _publisher.Publish(new CreateTransaction(10, "My Account 2"));
            await _publisher.Publish(new CreateTransaction(10, "My Account 1"));
            await _publisher.Publish(new CreateTransaction(10, "My Account 2"));

            await _appDbContext.SaveChangesAsync();
        }
    }
}
