﻿using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : Controller
    {
        public readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task<ActionResult> Index()
        {
            //Last 7 Days
            DateTime startDate = DateTime.Today.AddDays(-6);
            DateTime endDate = DateTime.Today;

            List<Transaction> selectedTransactions = await _context.Transactions.Include(x => x.Category).Where(y => y.Date >= startDate && y.Date <= endDate).ToListAsync();

            //Total Income
            int totalIncome = selectedTransactions.Where(i => i.Category.Type == "Income").Sum(j => j.Amount);

            ViewBag.totalIncome = totalIncome.ToString("C0");

            //Total Expense
            int totalExpense = selectedTransactions.Where(i => i.Category.Type == "Expense").Sum(j => j.Amount);

            ViewBag.totalExpense = totalExpense.ToString("C0");

            //Balance
            int balance = totalIncome - totalExpense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.balance = String.Format(culture, "{0:C0}", balance);

            return View();
        }
    }
}
