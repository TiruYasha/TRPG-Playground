using System;
using System.Collections.Generic;
using System.Text;
using Domain.Domain;

namespace Service.Test
{
    public static class DataCreator
    {
        public static Game GetGame()
        {
            var user = new User();

            var game = new Game("testgame", user);

            return game;
        }
    }
}
