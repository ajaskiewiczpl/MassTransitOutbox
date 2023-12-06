using MassTransit;
using MassTransitOutbox.Domain;
using Microsoft.EntityFrameworkCore;

namespace MassTransitOutbox.Infrastructure
{
    public class CreateTransactionConsumer : IConsumer<CreateTransaction>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CreateTransactionConsumer> _logger;

        public CreateTransactionConsumer(AppDbContext appDbContext, ILogger<CreateTransactionConsumer> logger)
        {
            _dbContext = appDbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateTransaction> context)
        {
            _logger.LogInformation("Consuming message ID: {messageId}", context.MessageId);

            BankAccount? account = await _dbContext.Accounts.SingleOrDefaultAsync(x => x.Name == context.Message.BankAccountName);

            if (account == null)
            {
                _logger.LogInformation("Account doesn't exist and will be auto created: {accountName}", context.Message.BankAccountName);

                account = new BankAccount
                {
                    Name = context.Message.BankAccountName
                };

                _dbContext.Accounts.Add(account);
            }

            Transaction transaction = new Transaction()
            {
                Amount = context.Message.Amount,
                Description = "description",
                Account = account
            };

            _dbContext.Transactions.Add(transaction);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Finished consuming message ID: {messageId}", context.MessageId);
        }
    }
}
