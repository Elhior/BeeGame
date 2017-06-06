using System;

namespace Bee_game.Models
{
    public class Queen:IBee 
    {
        //needed for load Bees from repository
        public Queen() { }

        public Queen(string name, int lifespan, int hitpoints) : base(name, lifespan, hitpoints)
        {
        }
    }
}