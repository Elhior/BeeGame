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
        IRepository<GameInstance> dbContext;

        public BeeGameService()
        {
            //dbContext = new SQLBeeRepository();
            dbContext = new MongoBeeRepository();
        }

        public void NewGame(int gameID)
        {
            GameInstance.getInstance(gameID).Stage.Clear();
        }

        public void SaveGame(int gameID, string savingtype)
        {
            //ref of game to avoid multiple getInstance method calls
            GameInstance game = GameInstance.getInstance(gameID);
            if (savingtype == "file")
            {
                StreamWriter strwtr = new StreamWriter(Path.GetTempPath() + "MyTest.txt");
                game.Stage.ForEach(strwtr.WriteLine);
                strwtr.Close();
            }

            if (savingtype == "repository")
            {
                dbContext.Save(game);
            }

            if (savingtype == "memory")
            {
                game.stream = new MemoryStream();
                string bees = string.Join("<?>]", game.Stage);
                byte[] buffer = Encoding.UTF8.GetBytes(bees);
                game.stream.Write(buffer, 0, buffer.Length);
            }
        }

        public string LoadGame(int gameID, string loadingtype)
        {
            //ref of game to avoid multiple getInstance method calls
            GameInstance game = GameInstance.getInstance(gameID);
            //clearing stage, if game is loaded when previos stage isn`t ended
            game.Stage.Clear();

            if (loadingtype == "file")
            {
                try
                {
                    //parse save_file and make game stage from its data
                    foreach (string bee in System.IO.File.ReadAllLines(Path.GetTempPath() + "MyTest.txt"))
                    {
                        string[] beeParams = bee.Split(':');

                        if (beeParams[0] == "Queen")
                            game.Stage.Add(new Bee(beeParams[0], Int32.Parse(beeParams[1]), Int32.Parse(beeParams[2])));
                        if (beeParams[0] == "Worker")
                            game.Stage.Add(new Bee(beeParams[0], Int32.Parse(beeParams[1]), Int32.Parse(beeParams[2])));
                        if (beeParams[0] == "Drone")
                            game.Stage.Add(new Bee(beeParams[0], Int32.Parse(beeParams[1]), Int32.Parse(beeParams[2])));
                    }
                    return "Loaded.";
                }
                catch (FileNotFoundException)
                {
                    return "Save is not found.";
                }
            }

            if (loadingtype == "repository")
            {
                if (dbContext.Load(game))
                    return "Loaded.";
                else
                    return "Save is not found.";
            }

            if (loadingtype == "memory")
            {
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

                    if (beeParams[0] == "Queen")
                        game.Stage.Add(new Bee(beeParams[0], Int32.Parse(beeParams[1]), Int32.Parse(beeParams[2])));
                    if (beeParams[0] == "Worker")
                        game.Stage.Add(new Bee(beeParams[0], Int32.Parse(beeParams[1]), Int32.Parse(beeParams[2])));
                    if (beeParams[0] == "Drone")
                        game.Stage.Add(new Bee(beeParams[0], Int32.Parse(beeParams[1]), Int32.Parse(beeParams[2])));
                }
                return "Loaded.";
            }
            return "Save is not found.";
        }

        //save new game configuration specified in view
        public void SaveConfiguration(int gameID, GameConfiguration gameConfiguration)
        {
            GameInstance.SetConfiguration(gameID, gameConfiguration);
        }

        //put current game configuration as default values in views edit_settings window
        public string GetConfiguration(int gameID)
        {
            return JsonConvert.SerializeObject(GameInstance.getInstance(gameID).gameConfig);
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
            return beeNumber + "LP" + game.Stage[beeNumber].Lifespan;
        }
    }
}