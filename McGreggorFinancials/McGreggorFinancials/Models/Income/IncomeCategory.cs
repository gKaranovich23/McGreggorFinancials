using System.ComponentModel.DataAnnotations;

namespace McGreggorFinancials.Models.Income
{
    public class IncomeCategory
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
