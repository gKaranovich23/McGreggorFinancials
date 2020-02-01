using McGreggorFinancials.Models.Targets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.Models.Accounts
{
    public class Account
    {
        public int ID { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        [ForeignKey("Type")]
        public int TypeID { get; set; }
        public virtual AccountType Type { get; set; }

        [ForeignKey("TargetAmount")]
        public int TargetID { get; set; }
        public virtual TargetAmount TargetAmount { get; set; }
    }
}
