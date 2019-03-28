using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Database
{
    public class DndUser : IdentityUser
    {
        public virtual List<Participant> Games { get; set; } 
    }
}