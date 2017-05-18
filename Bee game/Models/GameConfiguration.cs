using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bee_game.Models
{
    public class GameConfiguration
    {
        public int QueensNumber
        {
            get;
            set;
        }
        public int WorkersNumber
        {
            get;
            set;
        }
        public int DronesNumber
        {
            get;
            set;
        }
        public int QueensLifespan
        {
            get;
            set;
        }
        public int QueensHitpoints
        {
            get;
            set;
        }
        public int WorkersLifespan
        {
            get;
            set;
        }
        public int WorkersHitpoints
        {
            get;
            set;
        }
        public int DronesLifespan
        {
            get;
            set;
        }
        public int DronesHitpoints
        {
            get;
            set;
        }
        // default game configuration
        public GameConfiguration()
        {
            QueensNumber = 1;
            WorkersNumber = 5;
            DronesNumber = 8;
            QueensLifespan = 100;
            QueensHitpoints = 8;
            WorkersLifespan = 75;
            WorkersHitpoints = 10;
            DronesLifespan = 50;
            DronesHitpoints = 12;
        }
    }
}