using System;
using System.Web.Mvc;
using Bee_game.Models;
using Bee_game.Service;

namespace Bee_game.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Game()
        {
            return View(GameInstance.getInstance(GetBrowserId()).Stage);
        }

        [HttpGet]
        public void NewGame()
        {
            BeeGameService.Service().NewGame(this);
        }

        [HttpGet]
        public void SaveGame(string savingtype)
        {
            BeeGameService.Service().SaveGame(this, savingtype);
        }

        [HttpGet]
        public string LoadGame(string loadingtype)
        {
            return BeeGameService.Service().LoadGame(this, loadingtype);
        }

        [HttpGet]
        public string GetConfiguration()
        {
            return BeeGameService.Service().GetConfiguration(this);
        }

        [HttpPost]
        public void SaveConfiguration(GameConfiguration gameConfiguration)
        {
            BeeGameService.Service().SaveConfiguration(this,gameConfiguration);
        }

        [HttpGet]
        public string HitBee(string id)
        {
            return BeeGameService.Service().HitBee(this, id);
        }

        //get unique id of browser to run new games from different browsers simultaneously
        public int GetBrowserId()
        {
            try
            {
                if (Request.Cookies["BeeGameSettings"]["ID"] == null){}
            }
            catch (Exception)
            {
                Response.Cookies["BeeGameSettings"]["ID"] = GameInstance.GetNewGameId().ToString();
            }
 
            return Int32.Parse(Request.Cookies["BeeGameSettings"]["ID"]);
        }
    }
}