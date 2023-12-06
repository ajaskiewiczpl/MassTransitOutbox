namespace MassTransitOutbox.Infrastructure
{
    public record CreateTransaction(decimal Amount, string BankAccountName);
}
