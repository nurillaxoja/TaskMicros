using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TaskMIcros.Data;
using TaskMIcros.Models;
using TaskMIcros.Models.DTO;
using TaskMIcros.Models.GridDisplay;

namespace TaskMIcros.Controllers
{
    public class IncomesController : Controller
    {
        private readonly DataContext _context;

        public IncomesController(DataContext context)
        {
            _context = context;
        }

        // GET: Incomes
        public async Task<IActionResult> Index()
        {
            var res = _context.Incomes;





            var dataContext = _context.Incomes.Include(i => i.User);
            return View(await dataContext.ToListAsync());




        }

        // GET: Incomes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Incomes == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        // GET: Incomes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Incomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Salary,Rent,Other,Total,IncomeDate,UserId, Commentary,IncomeLastDate ")] Income income)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(income);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
           
            
            income.Total = income.Salary + income.Rent + income.Other;
            var incomeTime = income.IncomeDate.ToUniversalTime();
            var incomeLastTime = income.IncomeLastDate.ToUniversalTime();

            income.IncomeLastDate = incomeLastTime;
            income.IncomeDate = incomeTime;

            if (income.Total == 0)
            {
                ModelState.AddModelError("Error", "Total sum should not be equal to zero");
                return View(income);

            }

            _context.Incomes.Add(income);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));


            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", income.UserId);
            //return View(income);
        }

        // GET: Incomes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Incomes == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes.FindAsync(id);
            if (income == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", income.UserId);
            return View(income);
        }

        // POST: Incomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Salary,Rent,Other,Total,IncomeDate,UserId")] Income income)
        {
            if (id != income.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(income);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncomeExists(income.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", income.UserId);
            return View(income);
        }

        // GET: Incomes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Incomes == null)
            {
                return NotFound();
            }

            var income = await _context.Incomes
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (income == null)
            {
                return NotFound();
            }

            return View(income);
        }

        // POST: Incomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Incomes == null)
            {
                return Problem("Entity set 'DataContext.Incomes'  is null.");
            }
            var income = await _context.Incomes.FindAsync(id);
            if (income != null)
            {
                _context.Incomes.Remove(income);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncomeExists(int id)
        {
            return (_context.Incomes?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        [HttpGet]
        public JsonResult GetAllIncomesJson()
        {
            var _incomes = _context.Incomes;
            var _users = _context.Users;

            var lastRes = from user in _users
                          join income in _incomes
                          on user.Id equals income.UserId
                          select new
                          {
                              Id = income.Id,
                              Salary = income.Salary,
                              Rent = income.Rent,
                              Other = income.Other,
                              Total = income.Total,
                              IncomeDate = income.IncomeDate,
                              UserId = user.Id,
                              UserName = user.Name,
                              Commentary = income.Commentary,
                              LastDate = income.IncomeLastDate
                          };

            var data = new
            {
                Items = lastRes,
                TotalCount = lastRes.Count()
            };
            return new JsonResult(data);
        }

        [HttpPost]
        public HttpStatusCode UpdateIncome(Object request)
        {
            try
            {
                var models = JsonConvert.DeserializeObject<IEnumerable<Income>>(Request.Form.FirstOrDefault().Value);
                Income income = new Income();

                foreach (var item in models)
                {
                    income.Id = item.Id;
                    income.Salary = item.Salary;
                    income.Rent = item.Rent;
                    income.Other = item.Other;
                    income.Total = item.Total;
                    income.UserId = item.UserId;
                    income.IncomeDate = item.IncomeDate.ToUniversalTime();
                    income.Commentary = item.Commentary;
                    income.IncomeLastDate = item.IncomeLastDate.ToUniversalTime();
                }

                _context.Incomes.Update(income);
                var saveResult = _context.SaveChanges();

                if (saveResult == 0)
                    return HttpStatusCode.InternalServerError;
                
                return HttpStatusCode.OK;
            
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public HttpStatusCode CreateIncome(Object request)
        {
            var models = JsonConvert.DeserializeObject<IEnumerable<Income>>(Request.Form.FirstOrDefault().Value);
            Income income = new Income();

            return HttpStatusCode.BadRequest;
        }

        public string GetAllUsersJson(Object request)
        {
            //var models = JsonConvert.DeserializeObject<IEnumerable<Income>>(Request.Form.FirstOrDefault().Value);
            //Income income = new Income();
            var _users = _context.Users;

            var res = from user in _users
                      select new
                      {
                          id = user.Id,
                          name = user.Name,
                      };

            //var data = new
            //{
            //    Items = res,
            //    TotalCount = res.Count()
            //};
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(res);
            return json;
        }
        

    }
}
