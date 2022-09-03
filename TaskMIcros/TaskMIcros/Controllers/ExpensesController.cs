using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TaskMIcros.Data;
using TaskMIcros.Models;
using TaskMIcros.Models.DropDownList;

namespace TaskMIcros.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly DataContext _context;

        public ExpensesController(DataContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Expenses.Include(e => e.User);

            return View(await dataContext.ToListAsync());
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Expenses == null)
            {
                return NotFound();
            }

            var expenses = await _context.Expenses
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenses == null)
            {
                return NotFound();
            }

            return View(expenses);
        }

        // GET: Expenses/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total,Other,Entertainment,Intenet,Mobile,Transport,Food,ExpenseDate,UserId,ExpenseLastDate,Commentary")] Expenses expenses)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(expenses);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", expenses.UserId);
            expenses.Total = expenses.Other + expenses.Entertainment + expenses.Intenet + expenses.Mobile + expenses.Transport + expenses.Food;
            var expenseTime = expenses.ExpenseDate.ToUniversalTime();
            var expenseLastTime = expenses.ExpenseLastDate.ToUniversalTime();
            expenses.ExpenseDate = expenseTime;
            expenses.ExpenseLastDate = expenseLastTime;

            if (expenses.Total == 0)
            {
                ModelState.AddModelError("Error", "Total expenses should not be equal to zero");
                return View(expenses);
            }
            _context.Expenses.Add(expenses);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public ActionResult Craet2(string button)
        {
            
            
            return View();
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Expenses == null)
            {
                return NotFound();
            }

            var expenses = await _context.Expenses.FindAsync(id);
            if (expenses == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", expenses.UserId);
            return View(expenses);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Total,Other,Entertainment,Intenet,Mobile,Transport,Food,ExpenseDate,UserId")] Expenses expenses)
        {
            if (id != expenses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpensesExists(expenses.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", expenses.UserId);
            return View(expenses);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Expenses == null)
            {
                return NotFound();
            }

            var expenses = await _context.Expenses
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (expenses == null)
            {
                return NotFound();
            }

            return View(expenses);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Expenses == null)
            {
                return Problem("Entity set 'DataContext.Expenses'  is null.");
            }
            var expenses = await _context.Expenses.FindAsync(id);
            if (expenses != null)
            {
                _context.Expenses.Remove(expenses);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpensesExists(int id)
        {
            return (_context.Expenses?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        [HttpGet]
        public JsonResult GetAllExpensesJson()
        {
            var _expenses = _context.Expenses;
            var _users = _context.Users;

            var JoinList = from user in _users
                           join expenses in _expenses
                           on user.Id equals expenses.UserId
                           select new
                           {
                               Id = expenses.Id,
                               Total = expenses.Total,
                               Other = expenses.Other,
                               Entertainment = expenses.Entertainment,
                               Intenet = expenses.Intenet,
                               Mobile = expenses.Mobile,
                               Transport = expenses.Transport,
                               Food = expenses.Food,
                               ExpenseDate = expenses.ExpenseDate,
                               Commentary = expenses.Commentary,
                               ExpenseLastDate = expenses.ExpenseLastDate,
                               UserId = expenses.UserId,
                               UserName = user.Name
                           };

            var data = new
            {
                Items = JoinList,
                TotalCount = JoinList.Count()
            };

            return Json(data);

        }

        [HttpPost]
        public HttpStatusCode UpdateExpenses(Object request)
        {
            var a = Request.Form.FirstOrDefault().Value;

            var models = JsonConvert.DeserializeObject<IEnumerable<Expenses>>(Request.Form.FirstOrDefault().Value);
            Expenses expenses = new Expenses();

            foreach (var item in models)
            {
                expenses.Id = item.Id;
                expenses.Total = item.Total;
                expenses.Other = item.Other;
                expenses.Entertainment = item.Entertainment;
                expenses.Intenet = item.Intenet;
                expenses.Mobile = item.Mobile;
                expenses.Transport = item.Transport;
                expenses.Food = item.Food;
                expenses.ExpenseDate = item.ExpenseDate.ToUniversalTime();
                expenses.Commentary = item.Commentary;
                expenses.ExpenseLastDate = item.ExpenseLastDate.ToUniversalTime();
                expenses.UserId = item.UserId;
            }
            _context.Expenses.Update(expenses);
            var saveResult = _context.SaveChanges();

            if (saveResult == 0)
                return HttpStatusCode.InternalServerError;


            return HttpStatusCode.OK;
        }




    }
}
