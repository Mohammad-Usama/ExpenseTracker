using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        
    }
}
