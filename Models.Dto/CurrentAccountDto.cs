using System;
using System.Collections.Generic;

namespace Models.Dto
{
    public class CurrentAccountDto
    {
        public Guid AccountId { get; set; }
        public decimal Balance { get; set; }
        public DateTime DateOpened { get; set; }
        public Guid CustomerId { get; set; }
        public List<TransactionDto>? Transactions { get; set; }
    }
}