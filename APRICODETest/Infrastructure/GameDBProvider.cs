using APRICODETest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace APRICODETest.Infrastructure
{
    public class GameDBProvider : IGameProvider
    {
        private AppDBContext dBContext;

        public GameDBProvider (AppDBContext context)
        {
            dBContext = context;
        }

        #region AddGame
        //Добавление игры в БД. Проверка разработчка и списка жанров по имени с последующим поиском в БД, присвоением, или если результат не найден, то они добавляются в БД
        public bool AddGame(Game game)
        {
            if (game != null)
            {
                //Переопределение свойства разработчик из БД
                game.Developer = GetOrCreateDeveloper(game.Developer);

                //Поиск и переопределение свойства жанров из БД
                game.Genres = GetOrCreateGenres(game.Genres);

                dBContext.Games.Add(game);
                dBContext.SaveChanges();
                return true;
            }
            else return false;
        }
        #endregion

        #region DeleteGame
        //Удаление игры по ID
        public bool DeleteGame(int id)
        {
            if (dBContext.Games.Any(g => g.ID == id))
            {

                Game game = dBContext.Games.Find(id);
                dBContext.Games.Remove(game);
                dBContext.SaveChanges();
                return true;
            }
            else return false;
        }
        #endregion

        #region EditGame
        //Изменение названия игры, разработчик и жанров
        public bool EditGame(Game game)
        {
            Game gameEdit = dBContext.Games.Include(i => i.Genres).FirstOrDefault(g => g.ID == game.ID);
            if (gameEdit != null)
            {
                gameEdit.Name = game.Name;
                gameEdit.Developer = GetOrCreateDeveloper(game.Developer);
                gameEdit.Genres = GetOrCreateGenres(game.Genres);
                
                dBContext.SaveChanges();
                return true;
            }
            else return false;
        }
        #endregion

        #region GetGames
        //Выгрузка всех игр и данных для них из смежных таблиц
        public IEnumerable<Game> GetGames()
        {
            return dBContext.Games.Include(x => x.Developer).Include(x => x.Genres).AsNoTracking();
        }
        #endregion

        #region GetGamesByID
        //Метод возвращает игру по указанному ID
        public Game GetGameByID(int id)
        {
            return dBContext.Games.FirstOrDefault(g => g.ID == id);
        }
        #endregion

        #region GetGamesPerGenre
        //Выгрузка игр по определенному жанру
        public IEnumerable<Game> GetGamesPerGenre(string genreName)
        {
            return dBContext.Games.Where(g => g.Genres.Any(gen => gen.Name == genreName)).Include(x => x.Developer).Include(x => x.Genres).AsNoTracking();
        }
        #endregion

        #region Private Methods
        //Вспомогательные методы для поиска и добавления, в случае отсутствия в БД, разработчиков и жанров
        private Developer GetOrCreateDeveloper(Developer developer)
        {
            Developer developerResult = dBContext.Developers.FirstOrDefault(d => d.Name == developer.Name); //Поиск разработчика в БД
            if (developerResult == null) //Если не найден, то создаем новую запись в БД
            {
                Developer newDeveloper = new Developer() { Name = developer.Name };
                dBContext.Developers.Add(newDeveloper);
                return newDeveloper;
            }
            else return developerResult; //Если объект найден, то возвращаем его
        }

        private ICollection<Genre> GetOrCreateGenres(IEnumerable<Genre> genres)
        {
            List<Genre> genresResult = (List<Genre>)genres; 
            for (int i = 0; i < genresResult.Count; i++) //Перебор значений в коллекции List
            {
                if (dBContext.Genres.Any(g => genresResult[i].Name == g.Name)) //Если жанр найден в БД по имени, то присваиваем значение из БД 
                {
                    genresResult[i] = dBContext.Genres.First(gen => gen.Name == genresResult[i].Name);
                }
                else //Если зачение не найдено в БД, то добавляем как новый жанр
                {
                    Genre newGenre = new Genre() { Name = genresResult[i].Name };
                    dBContext.Genres.Add(newGenre);
                    genresResult[i] = newGenre;
                }
            }
            return genresResult;
        }
        #endregion
    }
}
