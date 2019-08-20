using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBLibraryApp.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string UnconfirmedEmail { get; set; }
    }
}
