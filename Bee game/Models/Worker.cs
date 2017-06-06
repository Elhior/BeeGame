using System;

namespace Bee_game.Models
{
    public class Worker : IBee
    {
        //needed for load Bees from repository
        public Worker() { }

        public Worker(string name, int lifespan, int hitpoints) : base(name, lifespan, hitpoints)
        {
        }
    }
}