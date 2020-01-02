using McGreggorFinancials.Models.Accounts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace McGreggorFinancials.ViewModels.Admin
{
    public class AccountFormViewModel
    {
        public Account Account { get; set; }
        public SelectList AccountTypes { get; set; }
        public SelectList TargetTypes { get; set; }

        [Required]
        public int SelectedTarget { get; set; }
    }
}
