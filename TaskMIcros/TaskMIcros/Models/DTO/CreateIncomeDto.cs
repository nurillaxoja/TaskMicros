namespace TaskMIcros.Models.DTO
{
    public class CreateIncomeDto
    {
        public int Salary { get; set; }
        public int Rent { get; set; }
        public int Other { get; set; }
        public int Total { get; set; }
        public DateTime IncomeDate { get; set; }
        public DateTime IncomeLastDate { get; set; }
        public string Commentary { get; set; }



    }
}
