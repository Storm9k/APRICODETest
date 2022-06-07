using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APRICODETest.Infrastructure;
using APRICODETest.Model;

namespace APRICODETest.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private GameDBProvider gameDBProvider;

        public MainController(AppDBContext appDBContext)
        {
            gameDBProvider = new GameDBProvider(appDBContext);
        }

        //GET метод получения всего списка игр
        [HttpGet]
        public IEnumerable<Game> GetGames()
        {
            return gameDBProvider.GetGames();
        }

        //GET метод получения игры по определенному ID
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return gameDBProvider.GetGameByID(id).ToString();
        }

        //POST метод добавления новой игры
        [HttpPost]
        public ActionResult<Game> Post(Game game)
        {
            if (gameDBProvider.AddGame(game)) return Ok(game);
            else return BadRequest();
        }

        //PUT метод изменения свойств игры
        [HttpPut]
        public ActionResult<Game> Put(Game game)
        {
            if (gameDBProvider.EditGame(game)) return Ok(game);
            else return BadRequest();
        }

        //DELETE метод удаления игры по определенному ID
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (gameDBProvider.DeleteGame(id)) return Ok(id);
            else return BadRequest();
        }

        //GET метод получения списка игр по определенному жанру
        [HttpGet("genre/{name}")]
        public ActionResult<IEnumerable<Game>> GetGamesPerGenre(string name)
        {
            IEnumerable<Game> result = gameDBProvider.GetGamesPerGenre(name);
            if (result.Count() > 0) return Ok(result);
            else return BadRequest(name);
        }
    }
}
