using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskMIcros.Data;
using TaskMIcros.Models;
using TaskMIcros.Models.DTO;

namespace TaskMIcros.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(ILogger<HomeController> logger, DataContext context)
        {
            _logger = logger;
            this._context = context;
        }

        public IActionResult Index()
        {
            #region to paly
            //GetIncomes(1);
            //var a = 1;
            //CreateIncomeDto incomeDto = new CreateIncomeDto {
            //    IncomeDate = DateTime.Now,
            //    Other = 7777, 
            //    Rent =0 ,
            //    Salary =0 ,
            //    Total = 0, 
            //};
            //AddIncome(incomeDto, 3);
           // ChangeIncome();
            ///expenses

            //GetExpenses(2);
            //CreateExpensesDto dto = new CreateExpensesDto
            //{
            //    ExpenseDate = DateTime.UtcNow,
            //    Entertainment = 563,
            //    Food = 3434,
            //    Intenet = 6,
            //    Mobile = 6,
            //    Other = 7,
            //    Total = 7,
            //    Transport = 6,
            //};
            //AddExpenses(dto, 2);

            ///Users

            //AddUser("madi"); 
            //GetUser(4);
            #endregion to paly


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region income async
        //public async Task<ActionResult<List<Income>>> GetIncomes(int userId)
        //public async Task<<List<Income>> GetIncomes(int userId)
        //{
        //    try
        //    {
        //        var incomes = await _context.Incomes
        //        .Where(c => c.UserId == userId).ToListAsync();
        //        return incomes;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}
        #endregion income async

        private List<Income> GetIncomes(int userId)
        {
            try
            {
                var incomes = _context.Incomes
                .Where(c => c.UserId == userId).ToList();
                var a = incomes;
                return incomes;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private List<Income> AddIncome(CreateIncomeDto request ,int userId)
        {
            try
            {
                // should be corrected universal date time; 
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return null;
                }
                DateTime dt = DateTime.Now.ToUniversalTime();

                var income = new Income
                {
                    IncomeDate = dt,
                    Salary = request.Salary,
                    Other = request.Other,
                    Rent = request.Rent,
                    Total = request.Total,
                    User = user,
                };

                _context.Incomes
                    .Add(income);
                _context.SaveChanges();

                return null;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private void ChangeIncome()
        {
            Income request = new Income
            {
                Id = 1,
                Salary = 99999,
                Rent = 0,
                Other = 0,
                Total = 0,
                UserId = 1,
                IncomeDate = DateTime.Now.ToUniversalTime(),
            };

            _context.Incomes.Update(request);
            _context.SaveChanges();
        }
        private List<Expenses> GetExpenses(int userId)
        {
            try
            {
                var expenses = _context.Expenses
                .Where(c => c.UserId == userId)
                .ToList();

                var a = expenses;

                return expenses;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }


        private List<Expenses> AddExpenses(CreateExpensesDto request, int userId)
        {
            var user = _context.Users
                .Find(userId);
            if (user == null)
            {
                return null;
            }
            DateTime dt = DateTime.Now.ToUniversalTime();

            var expenses = new Expenses
            {
                ExpenseDate = dt,
                Entertainment = request.Entertainment,
                Food = request.Food,
                Intenet = request.Intenet,
                Mobile = request.Mobile,
                Other = request.Other,
                Total = request.Total,
                Transport = request.Transport,
                User = user,
            }; 
            _context.Expenses.Add(expenses);
            _context.SaveChanges();

            return null;

        }

        private bool AddUser(string name)
        {
            try
            {
                User user = new User
                {
                    Name = name,
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                throw e;
            }
           
        }

        private User GetUser(int userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    return null;
                }
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}