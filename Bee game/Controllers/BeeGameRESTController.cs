using System;
using System.Web.Mvc;
using Bee_game.Models;
using Bee_game.Service;

namespace Bee_game.Controllers
{
    public class BeeGameRESTController : Controller
    {

        [HttpGet]
        public void NewGame()
        {
            BeeGameService.Service().NewGame(GetBrowserId());
        }

        [HttpGet]
        public void SaveGame(string savingtype)
        {
            BeeGameService.Service().SaveGame(GetBrowserId(), savingtype);
        }

        [HttpGet]
        public string LoadGame(string loadingtype)
        {
            return BeeGameService.Service().LoadGame(GetBrowserId(), loadingtype);
        }

        [HttpGet]
        public string GetConfiguration()
        {
            return BeeGameService.Service().GetConfiguration(GetBrowserId());
        }

        [HttpPost]
        public void SaveConfiguration(GameConfiguration gameConfiguration)
        {
            BeeGameService.Service().SaveConfiguration(GetBrowserId(), gameConfiguration);
        }

        [HttpGet]
        public string HitBee(string id)
        {
            return BeeGameService.Service().HitBee(GetBrowserId(), id);
        }

        //get unique id of browser to run new games from different browsers simultaneously
        public int GetBrowserId()
        {
            try
            {
                if (Request.Cookies["BeeGameSettings"]["ID"] == null) { }
            }
            catch (Exception)
            {
                Response.Cookies["BeeGameSettings"]["ID"] = GameInstance.GetNewGameId().ToString();
            }

            return Int32.Parse(Request.Cookies["BeeGameSettings"]["ID"]);
        }
    }
}