using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

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
            QueensNumber = Int32.Parse(ConfigurationManager.AppSettings["QueensNumber"]);
            WorkersNumber = Int32.Parse(ConfigurationManager.AppSettings["WorkersNumber"]);
            DronesNumber = Int32.Parse(ConfigurationManager.AppSettings["DronesNumber"]);
            QueensLifespan = Int32.Parse(ConfigurationManager.AppSettings["QueensLifespan"]);
            QueensHitpoints = Int32.Parse(ConfigurationManager.AppSettings["QueensHitpoints"]);
            WorkersLifespan = Int32.Parse(ConfigurationManager.AppSettings["WorkersLifespan"]);
            WorkersHitpoints = Int32.Parse(ConfigurationManager.AppSettings["WorkersHitpoints"]);
            DronesLifespan = Int32.Parse(ConfigurationManager.AppSettings["DronesLifespan"]);
            DronesHitpoints = Int32.Parse(ConfigurationManager.AppSettings["DronesHitpoints"]);
        }
    }
}