using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Domain.Domain
{
    public class User : IdentityUser
    {
        public virtual IList<Game> CreatedGames { get; set; }
        public virtual IList<GamePlayer> GamePlayers { get; set; }

     
    }
}
