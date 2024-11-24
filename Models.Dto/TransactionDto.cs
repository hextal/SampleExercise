using Models.Dto;

namespace Models.Dto
{
    public class TransactionDto
    {
        public Guid TransactionId { get; set; }
        public Guid AccountId { get; set; }
        public required string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public required string TransactionStatus { get; set; }
        public string? TransactionRemarks { get; set; }
        
    }
}