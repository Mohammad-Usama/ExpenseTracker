using System;

namespace ExpenseTracker.Data
{
    public class ExpenseDTO
    {
        public int ExpenseId { get; set; }
        public decimal Amount { get; set; }
        public string ExpenseDate { get; set; }
        public DateTime FilterDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
