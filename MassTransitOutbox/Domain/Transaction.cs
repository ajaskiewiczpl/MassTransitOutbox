namespace MassTransitOutbox.Domain
{
    public class Transaction
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public BankAccount Account { get; set; }

        public Guid AccountId { get; set; }
    }
}
