using APRICODETest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APRICODETest.Infrastructure
{
    interface IGameProvider
    {
        public bool AddGame(Game game);
        public IEnumerable<Game> GetGames();
        public bool EditGame(Game game);
        public bool DeleteGame(int id);

    }
}
