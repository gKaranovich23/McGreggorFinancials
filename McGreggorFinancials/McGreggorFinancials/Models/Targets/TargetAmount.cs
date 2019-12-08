using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Targets
{
    public class TargetAmount
    {
        public int ID { get; set; }

        [DisplayName("Target Amount:")]
        public int Amount { get; set; }

        [DisplayName("Target Percentage:")]
        public int Percentage { get; set; }

        [DisplayName("Target Type:")]
        [ForeignKey("TargetType")]
        public int TypeID { get; set; }

        public virtual TargetType TargetType { get; set; }
    }
}
