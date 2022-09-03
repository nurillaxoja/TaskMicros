using System.ComponentModel.DataAnnotations;

namespace TaskMIcros.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Income> Income { get; set; }

        public List<Expenses>  Expenses { get; set; }

    }
}
