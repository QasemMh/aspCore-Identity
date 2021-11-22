using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _1_app.Models
{
    public class RoleFormViewModel
    {

        [Required, MaxLength(250)]         //set length
        public string Name { get; set; }
    }
}
