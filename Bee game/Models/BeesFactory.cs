using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Bee_game.Models
{
    public class BeesFactory
    {
        public enum Bees
        {
            Queen, Worker, Drone
        }

        public static IBee CreateBee(string name, int lifespan, int hitpoints)
        {
            switch ((Bees)Enum.Parse(typeof(Bees), name))
             {
                 case Bees.Queen:
                     return new Queen(name, lifespan, hitpoints);
                 case Bees.Worker:
                     return new Worker(name, lifespan, hitpoints);
                case Bees.Drone:
                    return new Drone(name, lifespan, hitpoints);
                default:
                     return new Drone(name, lifespan, hitpoints);
             }
        }
        //methods like CreateBee() cant be called in MongoDB.Driver.Linq
        public static List<IBee> CreateBees(IMongoCollection<IBee> collection)
        {            
            List<IBee> Queens = collection.AsQueryable().Where(x => x.Name == "Queen").Select(x => new Queen(x.Name, x.Lifespan, x.Hitpoints)).ToList<IBee>();
            List<IBee> Workers = collection.AsQueryable().Where(x => x.Name == "Worker").Select(x => new Worker(x.Name, x.Lifespan, x.Hitpoints)).ToList<IBee>();
            List<IBee> Drones = collection.AsQueryable().Where(x => x.Name == "Drone").Select(x => new Drone(x.Name, x.Lifespan, x.Hitpoints)).ToList<IBee>();
            return Queens.Concat(Workers).Concat(Drones).ToList();
        }
    }
}