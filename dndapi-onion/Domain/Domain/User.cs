using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Domain
{
    public class User : IdentityUser<Guid>
    {
        public virtual ICollection<Game> CreatedGames { get; private set; }
        public virtual ICollection<GamePlayer> JoinedGames { get; private set; }
    }
}
