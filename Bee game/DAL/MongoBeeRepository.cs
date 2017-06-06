using System;
using System.Collections.Generic;
using System.Linq;
using Bee_game.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Configuration;

namespace Bee_game.DAL
{
    public class MongoBeeRepository : IRepository<List<IBee>>
    {
        MongoClient client;
        IMongoDatabase database;
        IMongoCollection<IBee> BeeCollection;

        public MongoBeeRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var con = new MongoUrlBuilder(connectionString);
            client = new MongoClient(connectionString);
            database = client.GetDatabase(con.DatabaseName);
            BeeCollection = database.GetCollection<IBee>("gamesave");
        }
        //saving game
        public bool Save(List<IBee> stage)
        {
            try
            {
                // cleaning save before writing current state of game
                BeeCollection.DeleteMany(x => x.Name != null);
                // writing current state of game into save
                stage.ForEach(bee => BeeCollection.InsertOne(bee));

                Logger.Log.Info("Game Saved.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Game saving failed:" + ex.ToString());
                return false;
            }
        }

        //loading game
        public bool Load(List<IBee> stage)
        {
            //load game state from repository
            var beesList = BeesFactory.CreateBees(BeeCollection);

            if (beesList.Count() == 0)
            {
                Logger.Log.Info("Game loading failed.Save not found.");
                return false;
            }
            //make game stage from repository_save data
            foreach (var bee in beesList)
            {
                stage.Add(bee);
            }
            Logger.Log.Info("Game Loaded");
            return true;
        }

        public void Dispose() { }
    }
}