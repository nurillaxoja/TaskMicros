using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TaskMIcros.Data;
using TaskMIcros.Models;
using TaskMIcros.Models.GridDisplay;

namespace TaskMIcros.Controllers
{
    public class UsersController : Controller
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'DataContext.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] User user)
        {

            if (user.Name == null)
            {
                return View(user);
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


            #region  original 
            /*
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
            */
            #endregion original 
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DataContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        [HttpGet]
        public JsonResult GetAllUsersJson()
        {
            double _incomeTotal;
            double _expensesTotal;

            var _users = _context.Users;
            var _incomes = _context.Incomes;
            var _expenses = _context.Expenses;

            Dictionary<int, string> usersList = new Dictionary<int, string>();
            List<UserGridDisplay> _grid = new List<UserGridDisplay>();

            foreach (var user in _users)
            {
                usersList.Add(user.Id, user.Name);
            }

            foreach (var user in usersList)
            {
                _incomeTotal = 0;
                _expensesTotal = 0;
                var totIncome = from income in _incomes
                                where income.UserId == user.Key
                                select income.Total;

                foreach (var inc in totIncome)
                {
                    _incomeTotal += inc;
                }

                var totExp = from expenses in _expenses
                             where expenses.UserId == user.Key
                             select expenses.Total;

                foreach (var exp in totExp)
                {
                    _expensesTotal += exp;
                }
                UserGridDisplay grid = new UserGridDisplay();
                grid.Id = user.Key;
                grid.Name = user.Value;
                grid.Income = _incomeTotal;
                grid.Expenses = _expensesTotal;

                _grid.Add(grid);
            }


            var res = _grid;

            var data = new
            {
                Items = res,
                TotalCount = res.Count()
            };

            return new JsonResult(data);
        }


        [HttpPost]
        public ActionResult UpdateUser(IEnumerable<UserGridDisplay> request)
        {
            var form = Request.Form;
            var models = JsonConvert.DeserializeObject<IEnumerable<UserGridDisplay>>(Request.Form.FirstOrDefault().Value);
            UserGridDisplay toUpdate = new UserGridDisplay();


            return null;

        }

        public List<Object> GetChartData()
        {
            List<Object> data = new List<Object>();
            List<string> labels = new List<string>
            {
                "Общий доход всех пользователей",
                "Суммарные расходы всех пользователей",
            };

            data.Add(labels);

            var totalIncome = _context.Incomes.Sum(x => x.Total);
            var totalExpenses = _context.Expenses.Sum(x => x.Total);
            List<int> dataTotal = new List<int>
            {
                totalIncome,
                totalExpenses,
            };
            data.Add(dataTotal);
            return data;
        }
    }

}
