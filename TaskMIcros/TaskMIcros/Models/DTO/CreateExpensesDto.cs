namespace TaskMIcros.Models.DTO
{
    public class CreateExpensesDto
    {
        public int Total { get; set; }
        public int Other { get; set; }
        public int Entertainment { get; set; }
        public int Intenet { get; set; }
        public int Mobile { get; set; }
        public int Transport { get; set; }
        public int Food { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
