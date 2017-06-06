using System;

namespace Bee_game.Models
{
    public class Drone : IBee
    {
        //needed for load Bees from repository
        public Drone() { }

        public Drone(string name, int lifespan, int hitpoints) : base(name, lifespan, hitpoints)
        {
        }
    }
}