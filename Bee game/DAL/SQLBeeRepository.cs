using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bee_game.Models;

namespace Bee_game.DAL
{
    public class SQLBeeRepository : IRepository<GameInstance>
    {
        private BeeContext db;

        public SQLBeeRepository()
        {
            db = new BeeContext();
        }
        //saving game
        public bool Save(GameInstance game)
        {
            try
            {
                // cleaning save before writing current state of game
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Bees]");
                // writing current state of game into save
                game.Stage.ForEach(bee => db.Bee.Add((Bee)bee));
                db.SaveChanges();

                Logger.Log.Info("Game Saved");
                return true;
            }
            catch (Exception)
            {
                Logger.Log.Error("Game saving failed.");
                return false;
            }
        }
        //loading game
        public bool Load(GameInstance game)
        {
            if (db.Bee.ToList().Count == 0)
            {
                Logger.Log.Info("Game loading failed.Save not found.");
                return false;
            }
            //make game stage from repository_save data
            db.Bee.ToList().ForEach(bee => game.Stage.Add(bee));
            Logger.Log.Info("Game Loaded");
            return true;
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}