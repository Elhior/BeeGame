using System;
using System.Collections.Generic;
using System.Linq;
using Bee_game.Models;

namespace Bee_game.DAL
{
    public class SQLBeeRepository : IRepository<List<IBee>>
    {
        private BeeContext db;

        public SQLBeeRepository()
        {
            db = new BeeContext();
        }
        //saving game
        public bool Save(List<IBee> stage)
        {
            try
            {
                try
                {
                    // cleaning save before writing current state of game
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Bees]");
                }
                catch (Exception ex)
                {
                    Logger.Log.Warn("TRUNCATE TABLE failed:" + ex.ToString());
                }

                // writing current state of game into save
                stage.ForEach(bee => db.Bee.Add(bee));
                db.SaveChanges();
                           
                Logger.Log.Info("Game Saved");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Game saving failed:"+ ex.ToString());
                return false;
            }
        }
        //loading game
        public bool Load(List<IBee> stage)
        {
            if (db.Bee.ToList().Count == 0)
            {
                Logger.Log.Info("Game loading failed.Save not found.");
                return false;
            }
            //make game stage from repository_save data
            db.Bee.ToList().ForEach(bee => stage.Add(bee));
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