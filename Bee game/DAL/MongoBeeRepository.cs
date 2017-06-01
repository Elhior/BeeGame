using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bee_game.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System.Configuration;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;

namespace Bee_game.DAL
{
    public class MongoBeeRepository : IRepository<GameInstance>
    {
        MongoClient client;
        IMongoDatabase database;
        IMongoCollection<Bee> collection;

        public MongoBeeRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var con = new MongoUrlBuilder(connectionString);
            client = new MongoClient(connectionString);
            database = client.GetDatabase(con.DatabaseName);
            collection = database.GetCollection<Bee>("gamesave");
        }
        //saving game
        public bool Save(GameInstance game)
        {
            try
            {
                // cleaning save before writing current state of game
                collection.DeleteMany(x => x.Name != null);
                // writing current state of game into save
                game.Stage.ForEach(bee => collection.InsertOne((Bee)bee));

                Logger.Log.Info("Game Saved.");
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
            //load game state from repository
            var list = collection.AsQueryable().Select(x => new Bee()
            {
                Name = x.Name,
                Lifespan = x.Lifespan,
                Hitpoints = x.Hitpoints
            });
            
            if (list.Count() == 0)
            {
                Logger.Log.Info("Game loading failed.Save not found.");
                return false;
            }
            //make game stage from repository_save data
            foreach (var bee in list)
            {
                game.Stage.Add(bee);
            }
            Logger.Log.Info("Game Loaded");
            return true;
        }

        public void Dispose() { }
    }
}