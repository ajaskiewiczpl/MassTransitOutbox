namespace MassTransitOutbox.Domain
{
    public class BankAccount
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public List<Transaction> Transactions { get; set; } = new();
    }
}
