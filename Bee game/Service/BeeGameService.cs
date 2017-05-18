using System;
using System.Collections.Generic;
using System.Linq;
using Bee_game.Models;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using Bee_game.Controllers;

namespace Bee_game.Service
{
    public class BeeGameService
    {
        private static BeeGameService service;

        private BeeGameService(){ }

        public static BeeGameService Service()
        {
            if (service == null)
                service = new BeeGameService();
            return service;
        }

        public void NewGame(HomeController controller)
        {
            GameInstance.getInstance(controller.GetBrowserId()).Stage.Clear();
        }

        public void SaveGame(HomeController controller, string savingtype)
        {
            //ref of game to avoid multiple getInstance method calls
            GameInstance game = GameInstance.getInstance(controller.GetBrowserId());
            if (savingtype == "file")
            {
                StreamWriter strwtr = new System.IO.StreamWriter(Path.GetTempPath() + "MyTest.txt");
                game.Stage.ForEach(strwtr.WriteLine);
                strwtr.Close();
            }

            if (savingtype == "repository")
            {
                Bee_game.DAL.BeeContext db = new Bee_game.DAL.BeeContext();
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE [Bees]");
                game.Stage.ForEach(bee => db.Bee.Add((Bee)bee));
                db.SaveChanges();
            }

            if (savingtype == "memory")
            {
                game.stream = new MemoryStream();
                string bees = string.Join("<?>]", game.Stage);
                byte[] buffer = Encoding.UTF8.GetBytes(bees);
                game.stream.Write(buffer, 0, buffer.Length);
            }
        }

        public string LoadGame(HomeController controller, string loadingtype)
        {
            //ref of game to avoid multiple getInstance method calls
            GameInstance game = GameInstance.getInstance(controller.GetBrowserId());
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
                Bee_game.DAL.BeeContext db = new Bee_game.DAL.BeeContext();
                if (db.Bee.ToList().Count == 0)
                    return "Save is not found.";
                //make game stage from repository_save data
                db.Bee.ToList().ForEach(bee => game.Stage.Add(bee));
                return "Loaded.";
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
        public void SaveConfiguration(HomeController controller, GameConfiguration gameConfiguration)
        {
            GameInstance.SetConfiguration(controller.GetBrowserId(),gameConfiguration);
        }

        //put current game configuration as default values in views edit_settings window
        public string GetConfiguration(HomeController controller)
        {
            return JsonConvert.SerializeObject(GameInstance.getInstance(controller.GetBrowserId()).gameConfig);
        }

        public string HitBee(HomeController controller, string id)
        {
            //ref of game to avoid multiple getInstance method calls
            GameInstance game = GameInstance.getInstance(controller.GetBrowserId());
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