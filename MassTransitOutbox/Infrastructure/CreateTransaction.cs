namespace MassTransitOutbox.Infrastructure
{
    public record CreateTransaction(decimal Amount, string Description, string BankAccountName);
}
