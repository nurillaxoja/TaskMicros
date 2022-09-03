namespace TaskMIcros.Models.GridDisplay
{
    public class IncomeGridDisplay
    {
        public int Id { get; set; }
        public int Salary { get; set; }
        public int Rent { get; set; }
        public int Other { get; set; }
        public int Total { get; set; }
        public DateTime IncomeDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
