using ExpenseTracker.Data;
using ExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.ViewModels
{
    public class ExpenseViewModel
    {
        public IEnumerable<Category> Categories { get; set; }

        public Expense Expense { get; set; }

        public IEnumerable<ExpenseDTO> Expenses { get; set; }
    }
}
