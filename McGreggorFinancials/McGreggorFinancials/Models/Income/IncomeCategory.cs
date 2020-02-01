using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace McGreggorFinancials.Models.Income
{
    public class IncomeCategory
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
