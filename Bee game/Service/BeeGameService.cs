using System;
using System.Collections.Generic;
using System.Linq;
using Bee_game.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Bee_game.DAL;

namespace Bee_game.Service
{
    public class BeeGameService : IService
    {
        IRepository<List<IBee>> dbContext;
        public enum SaveCase
        {
            File, Repository, Memory
        }

        public BeeGameService(IRepository<List<IBee>> context)
        {
            dbContext = context;
        }

        public void NewGame(int gameID)
        {            
            GameInstance.getInstance(gameID).Stage.Clear();
            Logger.Log.Info("New game started ID:" + gameID);
        }

        public void SaveGame(int gameID, string savingtype)
        {
            //ref of game to avoid multiple getInstance method calls
            GameInstance game = GameInstance.getInstance(gameID);

            switch ((SaveCase)Enum.Parse(typeof(SaveCase), savingtype))
            {
                case SaveCase.File:
                    StreamWriter strwtr = new StreamWriter(Path.GetTempPath() + "MyTest.txt");
                    game.Stage.ForEach(strwtr.WriteLine);
                    strwtr.Close();
                    break;
                case SaveCase.Repository:
                    dbContext.Save(game.Stage);
                    break;
                case SaveCase.Memory:
                    game.stream = new MemoryStream();
                    string bees = string.Join("<?>]", game.Stage);
                    byte[] buffer = Encoding.UTF8.GetBytes(bees);
                    game.stream.Write(buffer, 0, buffer.Length);
                    break;
                default:
                    Logger.Log.Warn("Unknown saving type request.");
                    break;
            }
            Logger.Log.Info("Gave saveing. ID:" + gameID);
        }

        public string LoadGame(int gameID, string loadingtype)
        {
            //ref of game to avoid multiple getInstance method calls
            GameInstance game = GameInstance.getInstance(gameID);
            //clearing stage, if game is loaded when previos stage isn`t ended
            game.Stage.Clear();

            switch ((SaveCase)Enum.Parse(typeof(SaveCase), loadingtype))
            {
                case SaveCase.File:
                    try
                    {
                        //parse save_file and make game stage from its data
                        foreach (string bee in System.IO.File.ReadAllLines(Path.GetTempPath() + "MyTest.txt"))
                        {
                            string[] beeParams = bee.Split(':');

                            game.Stage.Add(BeesFactory.CreateBee(beeParams[0], Int32.Parse(beeParams[1]), Int32.Parse(beeParams[2])));
                        }
                        Logger.Log.Info("Game loading. ID:" + gameID);
                        return "Loaded.";
                    }
                    catch (FileNotFoundException ex)
                    {
                        Logger.Log.Error(String.Format("File loading failed ID:{0}.\n{1}",gameID,ex.ToString()));
                        return "Save is not found.";
                    }

                case SaveCase.Repository:
                    if (dbContext.Load(game.Stage))
                        return "Loaded.";
                    else
                        return "Save is not found.";

                case SaveCase.Memory:
                    if (game.stream == null)
                        return "Save is not found.";
                    game.stream.Position = 0;
                    byte[] newBuffer = new byte[game.stream.Length];
                    game.stream.Read(newBuffer, 0, newBuffer.Length);
                    string newText = Encoding.UTF8.GetString(newBuffer);
                    List<string> newlist = newText.Split(new string[] { "<?>]" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    foreach (string bee in newlist)
                    {
                        string[] beeParams = bee.Split(':');

                        game.Stage.Add(BeesFactory.CreateBee(beeParams[0], Int32.Parse(beeParams[1]), Int32.Parse(beeParams[2])));
                    }
                    Logger.Log.Info("Game loading. ID:" + gameID);
                    return "Loaded.";
                default:
                    Logger.Log.Warn("Unknown loading type request.");
                    break;
            }
            return "Save is not found.";
        }

        //save new game configuration specified in view
        public void SaveConfiguration(int gameID, GameConfiguration gameConfiguration)
        {
            Logger.Log.Info("Changing game configuration. ID:" + gameID);
            GameInstance.SetConfiguration(gameID, gameConfiguration);
        }

        //put current game configuration as default values in views edit_settings window
        public GameConfiguration GetConfiguration(int gameID)
        {
            Logger.Log.Info("Requesting game configuration. ID:" + gameID);
            return GameInstance.getInstance(gameID).gameConfig;
        }

        public string HitBee(int gameID, string id)
        {
            //ref of game to avoid multiple getInstance method calls
            GameInstance game = GameInstance.getInstance(gameID);
            //number of bee to hit
            int beeNumber;
            //choice random not dead bee
            if (id == "random")
            {
                Random rnd = new Random();

                do
                {
                    beeNumber = rnd.Next(0, game.Stage.Count);
                }
                while (game.Stage[beeNumber].Lifespan <= 0);
            }
            else
                beeNumber = Int32.Parse(id);

            //chosen bees Lifespan is reduced
            game.Stage[beeNumber].Lifespan = game.Stage[beeNumber].Lifespan - game.Stage[beeNumber].Hitpoints;

            //check conditions of stage wining
            if (game.Stage[beeNumber].Lifespan <= 0 && game.Stage[beeNumber].Name == "Queen")
            {
                //clearing stage, when game is ended
                game.Stage.Clear();
                return "Victory";
            }
            Logger.Log.Error(String.Format("{0} bee is heating. ID:{1}", beeNumber, gameID));
            return beeNumber + "LP" + game.Stage[beeNumber].Lifespan;
        }
    }
}