using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskMIcros.Models.DropDownList
{
    public class ExpensesUserModel
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public int Other { get; set; }
        public int Entertainment { get; set; }
        public int Intenet { get; set; }
        public int Mobile { get; set; }
        public int Transport { get; set; }
        public int Food { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Commentary { get; set; }
        public DateTime ExpenseLastDate { get; set; }


        #region for drop down

        public List<SelectListItem> usersListDropDown { get; set; }

        public int UserId { get; set; }

        #endregion for drop down


    }
}
