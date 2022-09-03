using System.ComponentModel.DataAnnotations;

namespace TaskMIcros.Models
{
    public class Income
    {
        [Key]
        public int Id { get; set; }
        public int Salary { get; set; }
        public int Rent { get; set; }
        public int Other { get; set; }
        public int Total { get; set; }

        [DataType(DataType.Date)]
        public DateTime IncomeDate { get; set; }

        public string Commentary { get; set; }

        [DataType(DataType.Date)]
        public DateTime IncomeLastDate { get; set; }


        #region 1 : n 
        public User User { get; set; }
        public int UserId { get; set; }
        #endregion 1 : n



    }
}
