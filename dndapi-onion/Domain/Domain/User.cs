using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Domain
{
    public class User : IdentityUser<Guid>
    {
        public virtual IList<Game> CreatedGames { get; set; }
        public virtual IList<GamePlayer> GamePlayers { get; set; }
    }
}
