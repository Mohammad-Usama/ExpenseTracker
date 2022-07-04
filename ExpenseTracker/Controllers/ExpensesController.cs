using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Context;
using ExpenseTracker.Models;
using ExpenseTracker.ViewModels;
using ExpenseTracker.Data;

namespace ExpenseTracker.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public int ExpensDTO { get; private set; }

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            ExpenseViewModel model = new ExpenseViewModel();
            
            var expenseCatData = await (from exp in _context.Expenses
                     join cat in _context.Categories
                     on exp.CategoryId equals cat.Id
                     select new ExpenseDTO()
                     {
                         Amount = exp.Amount,
                         CategoryId = cat.Id,
                         CategoryName = cat.Name,
                         ExpenseDate = exp.ExpenseDate.ToString("dd-MM-yyyy"),
                         FilterDate = exp.ExpenseDate,
                         ExpenseId = exp.Id
                     }).ToListAsync();
            // yesterday date
            var startDate = DateTime.Now.AddDays(-1).Date;
            var endDate = DateTime.Now.Date;

            var data = expenseCatData.Where(x => x.FilterDate >= startDate && x.FilterDate <= endDate).ToList();

            model.Expenses = data;

            return View(model);
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // GET: Expenses/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new ExpenseViewModel();
            viewModel.Categories = await _context.Categories.ToListAsync();
            return View(viewModel);
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,ExpenseDate,CategoryId")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new ExpenseViewModel();
            viewModel.Categories = await _context.Categories.ToListAsync();

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            viewModel.Expense = expense;

            return View(viewModel);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,ExpenseDate,CategoryId")] Expense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expense);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseExists(expense.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }

        public async Task<IActionResult> GetFilters(DateTime startDate,DateTime endDate)
        {
            var expenseCatData = await(from exp in _context.Expenses
                                       join cat in _context.Categories
                                       on exp.CategoryId equals cat.Id
                                       select new ExpenseDTO()
                                       {
                                           Amount = exp.Amount,
                                           CategoryId = cat.Id,
                                           CategoryName = cat.Name,
                                           ExpenseDate = exp.ExpenseDate.ToString("dd-MM-yyyy"),
                                           FilterDate = exp.ExpenseDate,
                                           ExpenseId = exp.Id
                                       }).ToListAsync();

            var data = expenseCatData.Where(x => x.FilterDate >= startDate && x.FilterDate <= endDate).ToList();

            ExpenseViewModel model = new ExpenseViewModel();
            model.Expenses = data;

            return PartialView("_Expense", model);
        }
    }
}
