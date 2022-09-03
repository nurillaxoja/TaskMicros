using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaskMIcros.Models
{
    public class Expenses
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public int Other { get; set; }
        public int Entertainment { get; set; }
        public int Intenet { get; set; }
        public int Mobile { get; set; }
        public int Transport { get; set; }
        public int Food { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; }
        public string Commentary { get; set; }

        [DataType(DataType.Date)]
        public DateTime ExpenseLastDate { get; set; }

        #region 1 : n
        public User User { get; set; }
        public int UserId { get; set; }
        #endregion 1 : n
       

    }
}
