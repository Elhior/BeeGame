using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bee_game.Models
{
    public class GameInstance
    {
        //games from all browsers
        private static List<GameInstance> games;
        //Bees list of current stage
        public List<IBee> Stage;
        //game configuration
        public GameConfiguration gameConfig;
        //MemoryStream game can be saved in
        public MemoryStream stream;
        //unique id of game in browser
        private int gameId;

        // method to get game instance, create game or stage if doesn`t exist
        public static GameInstance getInstance(int browserId)
        {
            if (games == null)
            {
                games = new List<GameInstance>();
            }
            if (!games.Exists(n => n.gameId == browserId))
            {
                games.Add(new GameInstance(browserId));
            }
            if (games.First(n => n.gameId == browserId).Stage.Count == 0)
            {
                games.First(n => n.gameId == browserId).MakeStage();
            }
            return games.First(n => n.gameId == browserId);
        }

        //get unique id for browser
        public static int GetNewGameId()
        {
            try
            {
                return games.Max(n => n.gameId)+1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        // method to set game configuration
        public static void SetConfiguration(int browserId, GameConfiguration newGameConfig)
        {
            if (!games.Exists(n => n.gameId == browserId))
            {
                games.Add(new GameInstance(browserId, newGameConfig));
            }
            else
                games.First(n => n.gameId == browserId).gameConfig = newGameConfig;
        }

        // create game instance with specified configuration(if it`s needed only save game configuration,without stage get started)
        private GameInstance(int browserId, GameConfiguration gameConfiguration)
        {
            gameId = browserId;
            gameConfig = gameConfiguration;
            Stage = new List<IBee>();
        }

        // create game instance with default configuration, and stage started
        private GameInstance(int browserId)
        {
            gameId = browserId;
            gameConfig = new GameConfiguration();
            Stage = new List<IBee>();
            MakeStage();           
        }
        // create stage with current configuration
        private void MakeStage()
        {
            for (int i = 0; i < gameConfig.QueensNumber; i++)
            {
                Stage.Add(BeesFactory.CreateBee("Queen", gameConfig.QueensLifespan, gameConfig.QueensHitpoints));
            }
            for (int i = 0; i < gameConfig.WorkersNumber; i++)
            {
                Stage.Add(BeesFactory.CreateBee("Worker", gameConfig.WorkersLifespan, gameConfig.WorkersHitpoints));
            }
            for (int i = 0; i < gameConfig.DronesNumber; i++)
            {
                Stage.Add(BeesFactory.CreateBee("Drone", gameConfig.DronesLifespan, gameConfig.DronesHitpoints));
            }
        }
    }
}